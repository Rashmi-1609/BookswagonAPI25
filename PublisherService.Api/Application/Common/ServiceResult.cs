namespace PublisherService.Api.Application.Common;

// A generic wrapper for all API responses.
public class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }

    // Factory methods for creating success and failure results.
    public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static ServiceResult<T> Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
}
