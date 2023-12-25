namespace DotNet.TestProject.IdentityService.Application.Validators;

/// <summary>
///  Fluent Validation Intended for Register Command Model
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]).{7,}$";

    #pragma warning disable
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Firstname).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Lastname).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(7)
                                .Matches(PasswordRegex);   
        RuleFor(x => x.Email).NotEmpty().MinimumLength(7)
                                .EmailAddress();
        RuleFor(x => x.Address).NotEmpty().ChildRules(x =>
        {
            x.RuleFor(a => a.Country).NotEmpty().MinimumLength(3);
            x.RuleFor(a => a.State).NotEmpty().MinimumLength(3);
            x.RuleFor(a => a.City).NotEmpty().MinimumLength(3);
            x.RuleFor(a => a.Street).NotEmpty().MinimumLength(5);
        });
    }
}