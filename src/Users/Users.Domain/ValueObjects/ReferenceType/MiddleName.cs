using ErrorOr;
using FluentValidation.Results;
using System.Globalization;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record MiddleName
{
    public const int MinLength = 2;
    public const int MaxLength = 20;

    public string Value { get; }

    private MiddleName(string value) => Value = value;

    public static ErrorOr<MiddleName> Create(string middleName)
    {
        var validator = new MiddleNameValidator();
        ValidationResult validationResult = validator.Validate(new MiddleName(middleName));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        var titleCasedMiddleName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(middleName.ToLower());
        return new MiddleName(titleCasedMiddleName);
    }

    private MiddleName()
    {
    }
}
