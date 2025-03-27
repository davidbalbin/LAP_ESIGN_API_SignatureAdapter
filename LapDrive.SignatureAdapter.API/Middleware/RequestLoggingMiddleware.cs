namespace LapDrive.SignatureAdapter.API.Middleware;

/// <summary>
/// Middleware to log HTTP requests and responses
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">The logger</param>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
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
        // Log the request
        _logger.LogInformation(
            "HTTP {RequestMethod} {RequestPath} started",
            context.Request.Method,
            context.Request.Path);

        // Capture original body stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue down the pipeline
            var start = DateTime.UtcNow;
            await _next(context);
            var elapsed = DateTime.UtcNow - start;

            // Read the response body
            responseBody.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            // Log the response
            var statusCode = context.Response.StatusCode;
            var level = statusCode >= 500 ? LogLevel.Error : 
                statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

            _logger.Log(
                level,
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                statusCode,
                elapsed.TotalMilliseconds);

            // Copy back to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            // Always restore the original stream
            context.Response.Body = originalBodyStream;
        }
    }
}