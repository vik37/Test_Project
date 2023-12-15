namespace DotNet.TestProject.IdentityService.Application.Queries;

/// <summary>
/// 
/// </summary>
public interface IUserQuery
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserViewModel> GetUser(string id);
}