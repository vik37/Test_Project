namespace DotNet.TestProject.IdentityService.Application.Behavior;

/// <summary>
/// Logging Behavior Pipline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Logging Behavior Pipline Constructor
    /// </summary>
    /// <param name="logger"></param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logging Behavior Pipline Handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        _logger.LogInformation("Request {RequestType} Started",requestType);

        var response = await next();

        _logger.LogInformation("Request {RequestType} completed successfully", requestType);

        return response;
    }
}