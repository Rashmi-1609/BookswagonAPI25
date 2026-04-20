namespace PublisherService.Api.Application.Common;

// A generic wrapper for all API responses.
public class ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }

    // Private constructor forces the use of Factory methods
    private ServiceResult(bool isSuccess, T? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    // Factory methods for creating success and failure results.
    public static ServiceResult<T> Success(T data) => new(true, data, null);
    public static ServiceResult<T> Failure(string errorMessage) => new(false, default, errorMessage);
}
