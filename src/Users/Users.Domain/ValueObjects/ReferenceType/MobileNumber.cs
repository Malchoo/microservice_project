using ErrorOr;
using FluentValidation.Results;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;
public sealed record MobileNumber
{
    public const int MinLength = 5;
    public const int MaxLength = 15;
    public string Value { get; }

    private MobileNumber(string value) => Value = value;

    public static ErrorOr<MobileNumber> Create(string mobileNumber)
    {
        var validator = new MobileNumberValidator();
        ValidationResult validationResult = validator.Validate(new MobileNumber(mobileNumber));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.PropertyName,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        return new MobileNumber(mobileNumber);
    }

    private MobileNumber()
    {
    }
}
