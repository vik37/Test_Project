namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class RegisterCommandHandler : IRequestHandler<RegisterCommand,bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(UserManager<User> userManager, 
                                ILogger<RegisterCommandHandler> logger,
                                IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    ///  Command Handler Intended for Register
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="IdentityUserException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userByEmail is not null)
                throw new IdentityUserException("The user is already registered");

            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(err => $"{err.Code} - {err.Description}");

                _logger.LogWarning("User Registration Failed at the level of {CommandType}", typeof(RegisterCommandHandler));
                foreach (var message in errorMessages)
                {
                    _logger.LogError("Registration Error Message: {message}", message);
                }
            }

            await _userManager.AddToRoleAsync(user, "customer");

            _logger.LogInformation("Registration was Success");

            return result.Succeeded;
        }
        catch (IdentityUserException ex)
        {
            throw new IdentityUserException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}