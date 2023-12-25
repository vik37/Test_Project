namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public record RegisterCommand : IRequest<bool>
{
    public string Username { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }

    public GenderDto Gender { get; init; }

    public Address Address { get; init; }

    public RegisterCommand()
    {
        Address = new Address();
    }
}