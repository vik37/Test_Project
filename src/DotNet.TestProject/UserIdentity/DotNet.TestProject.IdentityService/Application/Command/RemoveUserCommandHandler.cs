namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, bool>
{
    private readonly IdentityUserDbContext _context;
    private readonly ILogger<RemoveUserCommandHandler> _logger;

    public RemoveUserCommandHandler(IdentityUserDbContext context, ILogger<RemoveUserCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    ///     Command Handler for Delete User from DB by ID
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>boolean</returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (user == null)
                return false;

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {username} was deleted successfully", user.UserName);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message,ex);
        }
    }
}