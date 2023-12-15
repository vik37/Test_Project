namespace DotNet.TestProject.IdentityService.Application.DomainEvents;

/// <summary>
/// 
/// </summary>
public class UserLoggedinSuccessfullyDomainEventHandler : INotificationHandler<UserLoggedinSuccessfullyDomainEvent>
{
    private readonly ILogger<UserLoggedinSuccessfullyDomainEventHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public UserLoggedinSuccessfullyDomainEventHandler(ILogger<UserLoggedinSuccessfullyDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(UserLoggedinSuccessfullyDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("A logged-in user {Username} has been sent for a history entry and to notify via email", notification.Username);

        await  Task.CompletedTask;
    }
}