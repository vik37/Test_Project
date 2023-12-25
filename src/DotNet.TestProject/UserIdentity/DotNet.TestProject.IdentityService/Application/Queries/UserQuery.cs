namespace DotNet.TestProject.IdentityService.Application.Queries;

#pragma warning disable
public class UserQuery : IUserQuery
{
    private readonly IdentityUserDbContext _db;

	#pragma warning disable
    public UserQuery(IdentityUserDbContext db)
    {
        _db = db;
    }

    /// <summary>
    ///	Get User By ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Single User Entity by ID</returns>
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