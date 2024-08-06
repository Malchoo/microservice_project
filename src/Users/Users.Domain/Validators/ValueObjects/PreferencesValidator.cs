using FluentValidation;
using Users.Domain.Errors.Preferences;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

internal class PreferencesValidator : AbstractValidator<Preferences>
{
    public PreferencesValidator()
    {
        //RuleFor(preferences => preferences.Id)
        //    .NotNull()
        //    .WithErrorCode(PreferencesErrors.MissingUserId.Code)
        //    .WithMessage(PreferencesErrors.MissingUserId.Description);
    }
}
