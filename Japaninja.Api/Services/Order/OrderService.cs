using Japaninja.Common.Options;
using Japaninja.Creators.OrderCreator;
using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Addresses;
using Japaninja.Models.Order;
using Japaninja.Repositories.Repositories.Cutlery;
using Japaninja.Repositories.Repositories.Order;
using Japaninja.Repositories.Repositories.Product;
using Japaninja.Repositories.Repositories.Restaurant;
using Japaninja.Repositories.Repositories.User.Couriers;
using Japaninja.Repositories.Repositories.User.Customers;
using Japaninja.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Japaninja.Services.Order;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderConfigurationOptions _orderConfigurationOptions;
    private readonly IOrderCreator _orderCreator;

    public OrderService(
        IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory,
        IOptions<OrderConfigurationOptions> orderConfigurationOptions,
        IOrderCreator orderCreator)
    {
        _orderCreator = orderCreator;
        _unitOfWork = unitOfWorkFactory.Create();
        _orderConfigurationOptions = orderConfigurationOptions.Value;
    }


    public async Task<OrderConfiguration> GetOrderConfigurationAsync(string customerId)
    {
        var restaurantRepository = _unitOfWork.GetRepository<Restaurant, RestaurantRepository>();
        var customersRepository = _unitOfWork.GetRepository<CustomerUser, CustomersRepository>();
        var cutleryRepository = _unitOfWork.GetRepository<Cutlery, CutleryRepository>();

        var customerAddresses = customerId is not null
            ? await customersRepository.GetCustomerAddressesAsync(customerId)
            : new List<CustomerAddress>();

        var mainRestaurant = restaurantRepository.GetQuery().FirstOrDefault();
        var availableCutlery = cutleryRepository.GetQuery().ToList();

        return new OrderConfiguration
        {
            DeliveryPrice = _orderConfigurationOptions.DeliveryPrice,
            MinDeliveryFreePrice = _orderConfigurationOptions.MinDeliveryFreePrice,
            Addressess = customerAddresses?.Select(_orderCreator.CreateFrom).ToList(),
            Cutlery = availableCutlery,
            SelfPickupRestaurant = mainRestaurant,
        };
    }

    public async Task<string> CreateOrderAsync(CreateOrder createOrder)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomersRepository>();
        var productRepository = _unitOfWork.GetRepository<DomainModel.Models.Product, ProductRepository>();

        var customer = await customerRepository.GetByIdAsync(createOrder.CustomerId);

        CustomerAddress customerAddress;

        if (customer is not null)
        {
            var existingCustomerAddresses = await customerRepository.GetCustomerAddressesAsync(customer.Id);
            var providedCustomerAddress = existingCustomerAddresses.FirstOrDefault(a => a.Id == createOrder.Address?.AddressId);

            if (providedCustomerAddress is not null)
            {
                customerAddress = providedCustomerAddress;
                customerAddress.Street = createOrder.Address?.Street;
                customerAddress.HouseNumber = createOrder.Address?.HouseNumber;
                customerAddress.FlatNumber = createOrder.Address?.FlatNumber;
                customerAddress.Entrance = createOrder.Address?.Entrance;
                customerAddress.Floor = createOrder.Address?.Floor;
            }
            else
            {
                customerAddress = new CustomerAddress
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customer.Id,
                    Street = createOrder.Address?.Street,
                    HouseNumber = createOrder.Address?.HouseNumber,
                    FlatNumber = createOrder.Address?.FlatNumber,
                    Entrance = createOrder.Address?.Entrance,
                    Floor = createOrder.Address?.Floor,
                };
            }
        }
        else
        {
            customerAddress = new CustomerAddress
            {
                Id = Guid.NewGuid().ToString(),
                Street = createOrder.Address?.Street,
                HouseNumber = createOrder.Address?.HouseNumber,
                FlatNumber = createOrder.Address?.FlatNumber,
                Entrance = createOrder.Address?.Entrance,
                Floor = createOrder.Address?.Floor,
            };
        }

        var order = new DomainModel.Models.Order
        {
            Id = Guid.NewGuid().ToString(),
            RestaurantId = createOrder.Restaurant?.Id,
            Customer = customer,
            Comment = createOrder.AdditionalInfo,
            DeliveryTime = createOrder.DeliveryTime,
            CustomerAddress = customerAddress,
            Status = OrderStatus.Processing,
            CustomerName = createOrder.Name,
            CustomerPhoneNumber = createOrder.Phone,
            CreatedAt = DateTime.Now,
        };

        order.Products = _orderCreator.CreateFrom(order, createOrder.Products);
        order.Cutlery = _orderCreator.CreateFrom(order, createOrder.Cutlery);

        var productsIds = order.Products.Select(p => p.ProductId).ToList();
        var products = await productRepository.GetQuery()
            .Where(p => productsIds.Contains(p.Id))
            .ToListAsync();

        var orderProducts = products.Select(p =>
        {
            return new
            {
                Product = p, createOrder.Products.FirstOrDefault(cp => cp.ProductId == p.Id)!.Amount,
            };
        }).ToList();

        order.Price = orderProducts.Sum(p => p.Product.Price * p.Amount);

        orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync();

        return order.Id;
    }

    public async Task<DomainModel.Models.Order> GetOrderAsync(string id)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetFullIncludedQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        return order;
    }

    public async Task<IReadOnlyCollection<DomainModel.Models.Order>> GetOrdersAsync(OrderStatus orderStatus)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var orders = await orderRepository.GetFullIncludedQuery()
            .Where(p => p.Status == orderStatus)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return orders;
    }

    public async Task<IReadOnlyCollection<DomainModel.Models.Order>> GetCustomerOrdersAsync(string customerId, bool isActiveOrders = true)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var baseQuery = orderRepository.GetFullIncludedQuery()
            .Where(p => p.CustomerId == customerId);

        if (!isActiveOrders)
        {
            baseQuery = baseQuery.Where(p => p.Status == OrderStatus.Closed);
        }
        else
        {
            baseQuery = baseQuery.Where(p => p.Status != OrderStatus.Closed);
        }

        var orders = await baseQuery.OrderByDescending(p => p.CreatedAt).ToListAsync();

        return orders;
    }

    public async Task<IReadOnlyCollection<DomainModel.Models.Order>> GetCouriersOrdersAsync(string courierId, OrderStatus orderStatus)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var orders = await orderRepository.GetFullIncludedQuery()
            .Where(p => p.Status == orderStatus && (p.CourierId == courierId || p.CustomerAddressId != null))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return orders;
    }

    public async Task CancelOrderAsync(string orderId)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);
        order.Status = OrderStatus.Canceled;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ProcessOrderAsync(string orderId)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);
        order.Status = OrderStatus.Preparing;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SetToReadyOrderAsync(string orderId)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);
        order.Status = OrderStatus.Ready;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ShipOrderAsync(string orderId, string courierId)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();
        var couriersRepository = _unitOfWork.GetRepository<CourierUser, CouriersRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);
        var courier = await couriersRepository.GetCourierAsync(courierId);

        var courierOrder = new CouriersOrders
        {
            Id = Guid.NewGuid().ToString(),
            Courier = courier,
            Order = order,
        };

        courier.Orders.Add(courierOrder);
        couriersRepository.Update(courier);

        order.CourierId = courierId;
        order.Status = OrderStatus.Shipping;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CloseOrderAsync(string orderId)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);

        order.Status = OrderStatus.Closed;
        order.DeliveryFactTime = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RateOrderAsync(string orderId, int rating, string feedback)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();

        var order = await orderRepository.GetByIdAsync(orderId);

        order.IsRated = true;
        order.Rating = rating;
        order.Feedback = feedback;

        await _unitOfWork.SaveChangesAsync();
    }
}