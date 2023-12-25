namespace DotNet.TestProject.IdentityService.Application.Behavior;

/// <summary>
/// Validation Behvior Pipline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest,TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    #pragma warning disable
    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, 
                                IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    /// <summary>
    /// Validation Behavior Pipline Handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType().Name;

        var failResults =  _validators.Select(v => v.Validate(request))
                                            .SelectMany(request => request.Errors)
                                            .Where(request => request.ErrorMessage is not null)
                                            .ToList();
        
        if(failResults.Any()) {
            _logger.LogWarning("Validation Errors - {CommandType} Command: {@Command} - Errors: {@ValidationErrors}", requestType, request, failResults);

            throw new IdentityUserException($"Validation Error of Request Type {requestType}", new ValidationException("Invalid Request",failResults)); 
        }

        return await next();
    }
}