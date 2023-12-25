namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class UpdateUserCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Username { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Email { get; init; }

    public Address Address { get; init; }

    public UpdateUserCommand()
    {
        Address = new Address();
    }
}