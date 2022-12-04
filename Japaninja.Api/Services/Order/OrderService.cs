using Japaninja.Common.Options;
using Japaninja.Creators.OrderCreator;
using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Order;
using Japaninja.Repositories.Repositories.Cutlery;
using Japaninja.Repositories.Repositories.Order;
using Japaninja.Repositories.Repositories.Restaurant;
using Japaninja.Repositories.Repositories.User.Customers;
using Japaninja.Repositories.UnitOfWork;
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
            Addressess = customerAddresses ?? new List<CustomerAddress>(),
            Cutlery = availableCutlery,
            SelfPickupRestaurant = mainRestaurant,
        };
    }

    public async Task<CreatedOrderInfo> CreateOrderAsync(CreateOrder createOrder)
    {
        var orderRepository = _unitOfWork.GetRepository<DomainModel.Models.Order, OrderRepository>();
        var customerRepository = _unitOfWork.GetRepository<CustomerUser, CustomersRepository>();

        var customer = await customerRepository.GetByIdAsync(createOrder.CustomerId);

        var customerAddress = new CustomerAddress
        {
            Id = Guid.NewGuid().ToString(),
            Address = createOrder.Address,
        };

        if (customer is not null)
        {
            var existingCustomerAddresses = await customerRepository.GetCustomerAddressesAsync(customer.Id);
            var providedCustomerAddress = existingCustomerAddresses.FirstOrDefault(a => a.Address == createOrder.Address);

            if (providedCustomerAddress is not null)
            {
                customerAddress = providedCustomerAddress;
            }
            else
            {
                customerAddress = new CustomerAddress
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customer.Id,
                    Address = createOrder.Address,
                };
            }
        }

        var order = new DomainModel.Models.Order
        {
            Id = Guid.NewGuid().ToString(),
            RestaurantId = createOrder.Restaurant.Id,
            Customer = customer,
            Comment = createOrder.AdditionalInfo,
            DeliveryTime = createOrder.DeliveryTime,
            CustomerAddress = customerAddress,
            Status = OrderStatus.Processing,
            CustomerName = createOrder.Name,
            CustomerPhoneNumber = createOrder.Phone
        };

        order.Products = _orderCreator.CreateFrom(order, createOrder.Products);
        order.Cutlery = _orderCreator.CreateFrom(order, createOrder.Cutlery);

        orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync();

        var createdOrder = await orderRepository.GetByIdAsync(order.Id);

        var createdOrderInfo = new CreatedOrderInfo
        {
            Number = createdOrder.NumberId,
            Address = createdOrder.CustomerAddress.Address,
            DeliveryTime = createdOrder.DeliveryTime,
        };

        return createdOrderInfo;
    }
}