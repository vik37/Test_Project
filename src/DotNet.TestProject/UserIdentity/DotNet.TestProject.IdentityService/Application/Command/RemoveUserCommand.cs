namespace DotNet.TestProject.IdentityService.Application.Command;

public class RemoveUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}