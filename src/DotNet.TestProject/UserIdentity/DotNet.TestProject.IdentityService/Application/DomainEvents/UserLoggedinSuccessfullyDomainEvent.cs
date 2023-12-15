namespace DotNet.TestProject.IdentityService.Application.DomainEvents;

/// <summary>
/// 
/// </summary>
/// <param name="model"></param>
/// <param name="token"></param>
public record UserLoggedinSuccessfullyDomainEvent: INotification
{
    public string UserIdentity { get; init; }

    public string Username { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Email { get; set; }

    public DateTime LoginDate { get; init; }

    public bool FirstimeRegistered { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIdentity"></param>
    /// <param name="username"></param>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="email"></param>
    /// <param name=""></param>
    public UserLoggedinSuccessfullyDomainEvent(string userIdentity, string username, string firstname, string lastname, string email
        )
    {
        UserIdentity = userIdentity;
        Username = username;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;

        LoginDate = DateTime.Now;

        FirstimeRegistered = false;
    }
};
