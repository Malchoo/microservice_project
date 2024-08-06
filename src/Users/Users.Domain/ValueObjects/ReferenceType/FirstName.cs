using ErrorOr;
using FluentValidation.Results;
using System.Globalization;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record FirstName
{
    public const int MinLength = 2;
    public const int MaxLength = 20;

    public string Value { get; }

    private FirstName(string value) => Value = value;

    public static ErrorOr<FirstName> Create(string firstName)
    {
        var validator = new FirstNameValidator();
        ValidationResult validationResult = validator.Validate(new FirstName(firstName));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.PropertyName,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        var titleCasedFirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.ToLower());
        return new FirstName(titleCasedFirstName);
    }

    private FirstName()
    {
    }
}
