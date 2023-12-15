namespace DotNet.TestProject.IdentityService.Application.Command;

/// <summary>
/// 
/// </summary>
public record RegisterCommand : IRequest<TokenDto>
{
    public string Username { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }

    public GenderDto Gender { get; init; }

    public Address Address { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public RegisterCommand()
    {
        Address = new Address();
    }
}