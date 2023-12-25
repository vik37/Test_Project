namespace DotNet.TestProject.IdentityService.Application.Command;

#pragma warning disable
public class RegisterCommandHandler : IRequestHandler<RegisterCommand,TokenDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(UserManager<User> userManager, 
                                ILogger<RegisterCommandHandler> logger,
                                IMapper mapper, IMediator mediator)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    ///  Command Handler Intended for Register
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="IdentityUserException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<TokenDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

            _logger.LogInformation("Login for the new Registered User Started");

            var logginCommand = new LoginCommand(request.Username, request.Password);
            var token = await _mediator.Send(logginCommand, cancellationToken);

            _logger.LogInformation("Login for the new Registered User Successfully Completed");
            return token;
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