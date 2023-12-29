namespace DotNet.TestProject.IdentityService.Infrastructure.GlobalExceptionHandling.ExceptionMiddlewares;

/// <summary>
/// Handling Errors Globally With the Custom Exception Middleware
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

#pragma warning disable
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

#pragma warning restore

    /// <summary>
    /// Check if everything goes well, the _next delegate should process the request and the Get action from our controller 
    /// should generate a successful response. 
    /// But if a request is unsuccessful, our middleware will trigger the catch block and call the HandleExceptionAsync method.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            int status = context.Response.StatusCode;

            if(status == (int)HttpStatusCode.Unauthorized || status == (int)HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Access Denied!");           

        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application.json";

        ErrorDetail errorDetail = new();

        switch (exception)
        {
            case UnauthorizedAccessException:
                if(context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorDetail.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorDetail.Message = $"{exception.Message} The user is unauthorized to enter the site. Login is required";
                    _logger.LogWarning("{exceptionMessage} User unauthorized attempt to access the site. StatusCode {statuscode}",exception.Message,errorDetail.StatusCode);
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorDetail.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorDetail.Message = $"{exception.Message} Only Administrator access is allowed. Forbidden for others.";
                    _logger.LogWarning("{exceptionMessage} Forbidden User (Not Admin) attempt to access the site. StatusCode {statuscode}", exception.Message, errorDetail.StatusCode);
                }
                break;
            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetail.StatusCode= (int)HttpStatusCode.BadRequest;
                errorDetail.Message = exception.Message;
                break;
            case IdentityUserException:
                if(exception.InnerException is ValidationException validationException)
                {
                    _logger.LogWarning("Invalid Object Model, {exception} - ({messages})",typeof(ValidationException),validationException.Errors);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDetail.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDetail.Message = "Error in form submission";
                    errorDetail.Messages.AddRange(validationException.Errors.Select(x => x.ErrorMessage).ToList());
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorDetail.StatusCode = (int)HttpStatusCode.NotFound;
                    errorDetail.Message = exception.Message;
                }
                break;
            case Exception:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetail.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetail.Message = "Internal Server Error from the custom middleware.";
                _logger.LogError("Sommething wrong happpend: ({exception})", exception);
                break;
        }

        await context.Response.WriteAsync(errorDetail.ToString());
    }
}