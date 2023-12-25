namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class LoginCommand : IRequest<TokenDto>
{
    public string Username { get; init; }
    public string Password { get; init; }

    public LoginCommand()
    {}

    public LoginCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }
}