namespace DotNet.TestProject.IdentityService.Application.Queries;

#pragma warning disable
public interface IUserQuery
{
    Task<UserViewModel> GetUser(string id);
}