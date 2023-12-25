namespace DotNet.TestProject.IdentityService.Application.Validators;

/// <summary>
/// Class Fluent Validation Intended for Update User Command Model
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    #pragma warning disable
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Firstname).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Lastname).NotEmpty().MinimumLength(3);
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