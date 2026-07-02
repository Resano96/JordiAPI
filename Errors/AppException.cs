namespace UserManagerAPI.Errors;

// Excepción personalizada que lleva su propio código HTTP.
// Equivale a la clase AppError de Express.
public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}