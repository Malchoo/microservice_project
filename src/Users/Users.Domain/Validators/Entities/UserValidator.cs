using FluentValidation;
using Users.Domain.Entities;
using Users.Domain.Errors;

namespace Users.Domain.Validators.Entities;


public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(aggregate => aggregate.Profile)
            .NotNull()
            .WithErrorCode(UserErrors.MissingUser.Code)
            .WithMessage(UserErrors.MissingUser.Description);

        RuleFor(aggregate => aggregate.Contacts)
            .NotNull()
            .WithErrorCode(UserErrors.MissingContacts.Code)
            .WithMessage(UserErrors.MissingContacts.Description);

        RuleFor(aggregate => aggregate.Settings)
            .NotNull()
            .WithErrorCode(UserErrors.MissingSettings.Code)
            .WithMessage(UserErrors.MissingSettings.Description);

        RuleFor(aggregate => aggregate.Preferences)
            .NotNull()
            .WithErrorCode(UserErrors.MissingPreferences.Code)
            .WithMessage(UserErrors.MissingPreferences.Description);
    }
}