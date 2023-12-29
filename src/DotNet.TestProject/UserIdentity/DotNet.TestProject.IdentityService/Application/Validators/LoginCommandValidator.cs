namespace DotNet.TestProject.IdentityService.Application.Validators;

#pragma warning disable
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    private const string Message = "Invalid Username or Password!!!";

    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage(Message);
        RuleFor(x => x.Password).NotEmpty().WithMessage(Message);
    }
}