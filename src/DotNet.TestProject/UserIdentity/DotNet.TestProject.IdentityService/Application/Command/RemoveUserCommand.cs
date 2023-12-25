namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class RemoveUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}