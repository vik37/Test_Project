namespace DotNet.TestProject.IdentityService.Application.Queries;

/// <summary>
/// 
/// </summary>
public class UserQuery : IUserQuery
{
    private readonly IdentityUserDbContext _db;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="db"></param>
    public UserQuery(IdentityUserDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UserViewModel> GetUser(string id)
    {
		try
		{
			var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);

			return user is not null? new UserViewModel
			{
				Id = user.Id,
				Username = user.UserName,
				Firstname = user.Firstname,
				Lastname = user.Lastname,
				Email = user.Email
			}:null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message,ex);
		}
    }
}