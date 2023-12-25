namespace DotNet.TestProject.IdentityService.Application.Command;

/// <summary>
/// Command Handler intended for User Login
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IMediator _mediator;

    #pragma warning disable
    public LoginCommandHandler(UserManager<User> userManager, ILogger<LoginCommandHandler> logger,
        ITokenGenerator tokenGenerator, IMediator mediator)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException();
        _mediator = mediator;
    }

    /// <summary>
    /// Command Handler to Handle User Login
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="IdentityUserException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            bool invalidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user is null || !invalidPassword)
            {
                _logger.LogWarning("Invalid Try to Logged In");
                throw new IdentityUserException("User does not exist or Password was Invalid!!!");
            }

            var roles = await _userManager.GetRolesAsync(user);

            UserClaims claims = new UserClaims
            {
                Id = user.Id,
                Username = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Role = roles.FirstOrDefault(),
            };

            var tokenModel = new TokenDto();
                
            tokenModel.Token = _tokenGenerator.Generator(claims);

            _logger.LogInformation("User Successfully Logged");

            var successfullyLoggedUserDomainEvent = new UserLoggedinSuccessfullyDomainEvent(user.Id,user.UserName,
                                                        user.Firstname,user.Lastname,user.Email);

            await _mediator.Publish(successfullyLoggedUserDomainEvent);

            return tokenModel;
        }
        catch(IdentityUserException ex)
        {
            throw new IdentityUserException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message,ex);
        }
    }
}