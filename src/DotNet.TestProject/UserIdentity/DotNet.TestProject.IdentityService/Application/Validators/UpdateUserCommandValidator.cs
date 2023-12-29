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
                string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(username);

                if (!string.IsNullOrEmpty(forbiddenWord))
                    context.AddFailure(nameof(username), $"Word {forbiddenWord}  is forbidden for the {nameof(username)}");
            });

        RuleFor(x => x.Firstname).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("Firstname: "+OnlyLettersMessage)
                                    .Custom((firstname, context) =>
                                    {
                                        string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(firstname);

                                        if (!string.IsNullOrEmpty(forbiddenWord))
                                            context.AddFailure(nameof(firstname), $"Word {forbiddenWord}  is forbidden for the {nameof(firstname)}");
                                    });

        RuleFor(x => x.Lastname).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("Lastname: "+OnlyLettersMessage)
                                    .Custom((lastname, context) =>
                                    {
                                        string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(lastname);

                                        if (!string.IsNullOrEmpty(forbiddenWord))
                                            context.AddFailure(nameof(lastname), $"Word {forbiddenWord}  is forbidden for the {nameof(lastname)}");
                                    });

        RuleFor(x => x.Email).NotEmpty().MinimumLength(7)
                                .EmailAddress();

        RuleFor(x => x.Address).NotNull().ChildRules(x =>
        {
            x.RuleFor(a => a.Country).NotEmpty().MinimumLength(3)
                                        .Matches(OnlyLettersRegex).WithMessage("Country: "+OnlyLettersMessage)
                                        .Custom((country, context) =>
                                        {
                                            string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(country);

                                            if (!string.IsNullOrEmpty(forbiddenWord))
                                                context.AddFailure(nameof(country), $"Word {forbiddenWord}  is forbidden for the {nameof(country)}");
                                        });

            x.RuleFor(a => a.State).NotEmpty().MinimumLength(3)
                                        .Matches(OnlyLettersRegex).WithMessage("State: "+OnlyLettersMessage)
                                        .Custom((state, context) =>
                                         {
                                             string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(state);

                                             if (!string.IsNullOrEmpty(forbiddenWord))
                                                 context.AddFailure(nameof(state), $"Word {forbiddenWord}  is forbidden for the {nameof(state)}");
                                         });

            x.RuleFor(a => a.City).NotEmpty().MinimumLength(3)
                                    .Matches(OnlyLettersRegex).WithMessage("City: "+OnlyLettersMessage)
                                    .Custom((city, context) =>
                                    {
                                        string forbiddenWord = ForbiddenWords.ForbiddenWordChecker(city);

                                        if (!string.IsNullOrEmpty(forbiddenWord))
                                            context.AddFailure(nameof(city), $"Word {forbiddenWord}  is forbidden for the {nameof(city)}");
                                    });

            x.RuleFor(a => a.Street).NotEmpty().MinimumLength(5);
        });
    }
}