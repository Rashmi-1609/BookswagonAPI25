using HotChocolate.Execution;

namespace PublisherService.Api.GraphQL.Filters;

/// <summary>
/// Intercepts all GraphQL errors globally before they are sent to the client,
/// logging the real technical issue and sanitizing the output for security.
/// </summary>
public class GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        // 1. If it's a controlled GraphQLException (like the ones we will manually throw in our Queries), 
        // we allow it to pass through untouched because we already wrote a safe error message.
        if (error.Exception is GraphQLException)
        {
            return error;
        }

        // 2. For all unexpected crashes (like database timeouts or null references),
        // log the scary technical details securely for the backend team.
        if (error.Exception is not null)
        {
            logger.LogError(error.Exception, "Unhandled GraphQL execution error: {Message}", error.Exception.Message);
        }

        // 3. Hide the stack trace and DB details from the frontend React team/customers,
        // returning a sanitized, professional message.
        return error.WithMessage("An unexpected system error occurred. The Bookswagon engineering team has been notified.")
                    .WithException(null) // Clear the exception so stack trace/details are not serialized
                    .WithCode("INTERNAL_SERVER_ERROR");
    }
}
