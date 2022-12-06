namespace Japaninja.Models.Error;

public enum ApiError
{
    UserDoesNotExist,
    UserWithTheSameEmailAlreadyExist,
    PasswordIsInvalid,

    OrderShouldBeInProcessingStatus,
    OrderShouldBeInPreparingStatus,
    YouCanNotCancelOrderNotInProcessingStatus,
}