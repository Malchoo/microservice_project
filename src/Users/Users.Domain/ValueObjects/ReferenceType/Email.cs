using ErrorOr;
using FluentValidation.Results;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record Email
{
    public const int MinLength = 4;
    public const int MaxLength = 254;

    public string Value { get; init; }

    private Email(string value) => Value = value;

    public static ErrorOr<Email> Create(string email)
    {
        var validator = new EmailValidator();
        ValidationResult validationResult = validator.Validate(new Email(email));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        return new Email(email);
    }

    private Email()
    {
    }
}
