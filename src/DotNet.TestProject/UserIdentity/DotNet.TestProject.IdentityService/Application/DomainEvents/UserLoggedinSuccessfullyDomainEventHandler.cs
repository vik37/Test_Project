namespace DotNet.TestProject.IdentityService.Application.DomainEvents;

#pragma warning disable
public class UserLoggedinSuccessfullyDomainEventHandler : INotificationHandler<UserLoggedinSuccessfullyDomainEvent>
{
    private readonly ILogger<UserLoggedinSuccessfullyDomainEventHandler> _logger;

    public UserLoggedinSuccessfullyDomainEventHandler(ILogger<UserLoggedinSuccessfullyDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// User Loggedin Successfully Domain Event Handler
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Completed Task</returns>
    public async Task Handle(UserLoggedinSuccessfullyDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("A logged-in user {Username} has been sent for a history entry and to notify via email", notification.Username);

        await  Task.CompletedTask;
    }
}