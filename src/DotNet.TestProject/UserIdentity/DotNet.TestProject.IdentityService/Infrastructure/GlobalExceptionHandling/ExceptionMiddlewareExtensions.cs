using Microsoft.AspNetCore.Diagnostics;

namespace DotNet.TestProject.IdentityService.Infrastructure.GlobalExceptionHandling;

#pragma warning disable
public static class ExceptionMiddlewareExtensions
{
#pragma warning restore
    /// <summary>
    /// Global Exception Handler
    /// Extension method in which a UseExceptionHandler middleware is registered. 
    /// A status code and response content type are populated, log the error message, 
    /// and return the response with the custom-created object.
    /// </summary>
    /// <param name="app">Type: IApplicationBuilder</param>
    public static void ExceptionHandlerConfiguration(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(err =>
        {
            err.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if(contextFeature is not null)
                {
                    Log.Error("Something wrong happend: {context}", contextFeature);

                    await context.Response.WriteAsync(new ErrorDetail()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error"
                    }.ToString());
                }
            });
        });
    }
}