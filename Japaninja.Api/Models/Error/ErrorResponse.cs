namespace Japaninja.Models.Error;

public class ErrorResponse
{
    public static ErrorResponse CreateFromApiError(ApiError apiError)
    {
        var error = Enum.GetName(apiError);

        return new ErrorResponse
        {
            Error = error,
        };
    }

    public string Error { get; set; }
}