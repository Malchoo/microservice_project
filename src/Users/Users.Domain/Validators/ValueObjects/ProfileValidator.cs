using FluentValidation;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class ProfileValidator : AbstractValidator<Profile>
{
    public ProfileValidator()
    {
        //RuleFor(user => user.Id)
        //    .NotNull()
        //    .WithErrorCode(ProfileErrors.MissingUserId.Code)
        //    .WithMessage(ProfileErrors.MissingUserId.Description);
    }
}

