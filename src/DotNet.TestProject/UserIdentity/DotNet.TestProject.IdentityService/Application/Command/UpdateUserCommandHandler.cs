namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IdentityUserDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IdentityUserDbContext context, ILogger<UpdateUserCommandHandler> logger, 
                    UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    ///   Command Handler Intended for Updating Single User Values
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Boolean</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userToEdit = await _context.User.SingleOrDefaultAsync(x => x.Id == request.Id);
            var userMG = await _userManager.FindByIdAsync(request.Id);

            if (userMG is null || userToEdit is null)
                throw new IdentityUserException("User Does Not Exist");

            userToEdit.Email = request.Email;
            userToEdit.Firstname = request.Firstname;
            userToEdit.Lastname = request.Lastname;
            userToEdit.UserName = request.Username;
            userToEdit.Address = request.Address;

            await _userManager.UpdateAsync(userToEdit);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Update was successfully");
            return true;
        }
        catch(IdentityUserException ex)
        {
            _logger.LogWarning(ex.Message);
            throw new IdentityUserException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message,ex);
        }
    }
}