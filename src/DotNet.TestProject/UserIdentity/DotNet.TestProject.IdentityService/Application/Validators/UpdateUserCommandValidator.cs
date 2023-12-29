namespace DotNet.TestProject.IdentityService.Application.Validators;

/// <summary>
/// Class Fluent Validation Intended for Update User Command Model
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private const string OnlyLettersRegex = @"^[A-Za-z\s]*$";
    private const string OnlyLettersMessage = "Only upper and lower case letters are allowed";
#pragma warning disable
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3)
            .Custom((username, context) =>
            {
                string tempWord = ForbiddenWords.IsThereAForbiddenWord(username);

                if (!string.IsNullOrEmpty(tempWord))
                    context.AddFailure(nameof(username), $"Word {tempWord}  is forbidden for the {nameof(username)}");
            });

        RuleFor(x => x.Firstname).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("Firstname: "+OnlyLettersMessage)
                                    .Custom((firstname, context) =>
                                    {
                                        string tempWord = ForbiddenWords.IsThereAForbiddenWord(firstname);

                                        if (!string.IsNullOrEmpty(tempWord))
                                            context.AddFailure(nameof(firstname), $"Word {tempWord}  is forbidden for the {nameof(firstname)}");
                                    });

        RuleFor(x => x.Lastname).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("Lastname: "+OnlyLettersMessage)
                                    .Custom((lastname, context) =>
                                    {
                                        string tempWord = ForbiddenWords.IsThereAForbiddenWord(lastname);

                                        if (!string.IsNullOrEmpty(tempWord))
                                            context.AddFailure(nameof(lastname), $"Word {tempWord}  is forbidden for the {nameof(lastname)}");
                                    });

        RuleFor(x => x.Email).NotEmpty().MinimumLength(7)
                                .EmailAddress();

        RuleFor(x => x.Address).NotNull().ChildRules(x =>
        {
            x.RuleFor(a => a.Country).NotEmpty().MinimumLength(3)
                                        .Matches(OnlyLettersRegex).WithMessage("Country: "+OnlyLettersMessage)
                                        .Custom((country, context) =>
                                        {
                                            string tempWord = ForbiddenWords.IsThereAForbiddenWord(country);

                                            if (!string.IsNullOrEmpty(tempWord))
                                                context.AddFailure(nameof(country), $"Word {tempWord}  is forbidden for the {nameof(country)}");
                                        });

            x.RuleFor(a => a.State).NotEmpty().MinimumLength(3)
                                        .Matches(OnlyLettersRegex).WithMessage("State: "+OnlyLettersMessage)
                                        .Custom((state, context) =>
                                         {
                                             string tempWord = ForbiddenWords.IsThereAForbiddenWord(state);

                                             if (!string.IsNullOrEmpty(tempWord))
                                                 context.AddFailure(nameof(state), $"Word {tempWord}  is forbidden for the {nameof(state)}");
                                         });

            x.RuleFor(a => a.City).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("City: "+OnlyLettersMessage)
                                    .Custom((city, context) =>
                                    {
                                        string tempWord = ForbiddenWords.IsThereAForbiddenWord(city);

                                        if (!string.IsNullOrEmpty(tempWord))
                                            context.AddFailure(nameof(city), $"Word {tempWord}  is forbidden for the {nameof(city)}");
                                    });

            x.RuleFor(a => a.Street).NotEmpty().MinimumLength(5);
        });
    }
}