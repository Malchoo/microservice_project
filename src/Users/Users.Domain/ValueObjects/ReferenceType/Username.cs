using ErrorOr;
using FluentValidation.Results;
using System.Globalization;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;
public sealed record Username
{
    public const int MinLength = 2;
    public const int MaxLength = 20;

    public string Value { get; }

    private Username(string value) => Value = value;

    public static ErrorOr<Username> Create(string username)
    {
        var validator = new UsernameValidator();
        ValidationResult validationResult = validator.Validate(new Username(username));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.PropertyName,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        var titleCasedFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username.ToLower());
        return new Username(titleCasedFirstName);
    }

    private Username()
    {
    }
}

