using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.Exceptions;
using System.Net;
using System.Text.Json;

namespace LapDrive.SignatureAdapter.API.Middleware;

/// <summary>
/// Middleware to handle exceptions globally across the application
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">The logger</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">The HttpContext for the current request</param>
    /// <returns>A task that represents the completion of request processing</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request processing");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = CommonConstants.ContentTypes.ApplicationJson;
        
        var response = new
        {
            error = new
            {
                message = exception.Message,
                detail = exception is BusinessException || exception is ValidationException || exception is DataException 
                    ? exception.Message 
                    : "An unexpected error occurred"
            }
        };

        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            BusinessException => (int)HttpStatusCode.BadRequest,
            DataException => (int)HttpStatusCode.InternalServerError,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}