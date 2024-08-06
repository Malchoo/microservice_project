using FluentValidation;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class MobileNumberValidator : AbstractValidator<MobileNumber>
{
    public MobileNumberValidator()
    {
        RuleFor(mobileNumber => mobileNumber.Value)
            .NotEmpty()
            .WithErrorCode(MobileNumberErrors.Empty.Code)
            .WithMessage(MobileNumberErrors.Empty.Description)

            .MinimumLength(MobileNumber.MinLength)
            .WithErrorCode(MobileNumberErrors.TooShort(MobileNumber.MinLength).Code)
            .WithMessage(MobileNumberErrors.TooShort(MobileNumber.MinLength).Description)


            .MaximumLength(MobileNumber.MaxLength)
            .WithErrorCode(MobileNumberErrors.TooLong(MobileNumber.MaxLength).Code)
            .WithMessage(MobileNumberErrors.TooLong(MobileNumber.MaxLength).Description)

            .Matches(@"^\+?\d{10,15}$")
            .WithErrorCode(MobileNumberErrors.Invalid.Code)
            .WithMessage(MobileNumberErrors.Invalid.Description)

            .Must(mobileNumber => !mobileNumber.Contains(" "))
            .WithErrorCode(MobileNumberErrors.NoWhiteSpaces.Code)
            .WithMessage(MobileNumberErrors.NoWhiteSpaces.Description);
    }
}
