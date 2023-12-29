namespace DotNet.TestProject.IdentityService.Infrastructure.Filters;

#pragma warning disable
public class CheckUserIdIArgumentAsyncFilter : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Filter for Checking the Correctness of the UserID Argument
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Log.Information("{filter} - {filtermethod} During Execution", typeof(CheckUserIdIArgumentAsyncFilter), nameof(OnActionExecutionAsync));
        
        if (context.ActionArguments.ContainsKey("userId"))
        {
            string userId = context.ActionArguments["userId"].ToString();

            if (string.IsNullOrEmpty(userId))
            {
                Log.Warning("Trying to access without User ID or UserID was Empty");
                throw new ArgumentException("User ID can not be empty");
            }

            bool doesUserIdHaveCorrectType = Guid.TryParse(userId, out Guid guid);

            if (!doesUserIdHaveCorrectType)
            {
                Log.Warning("Trying to access with incorrect type of Argument - User ID");
                throw new ArgumentException("Incorrect User ID");
            }
                
        }

        await next();

        Log.Information("{filter} - {filtermethod} After Execution", typeof(CheckUserIdIArgumentAsyncFilter), nameof(OnActionExecutionAsync));
    }
}