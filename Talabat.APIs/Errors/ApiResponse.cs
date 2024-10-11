namespace Talabat.APIs.Errors;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ApiResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string? GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request: The request was invalid or cannot be served.",
            401 => "Unauthorized: You need to authenticate to access this resource.",
            403 => "Forbidden: You do not have permission to access this resource.",
            404 => "Not Found: The resource you are looking for could not be found.",
            500 => "Internal Server Error: An unexpected error occurred. Please try again later.",
            _ => "An unexpected error occurred."
        };
    }
}