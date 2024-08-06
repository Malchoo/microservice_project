using ErrorOr;
using FluentValidation.Results;
using System.Globalization;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record LastName
{
    public const int MinLength = 1;
    public const int MaxLength = 20;

    public string Value { get; }

    private LastName(string value) => Value = value;

    public static ErrorOr<LastName> Create(string lastName)
    {
        var validator = new LastNameValidator();
        ValidationResult validationResult = validator.Validate(new LastName(lastName));

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        var titleCasedLastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.ToLower());
        return new LastName(titleCasedLastName);
    }

    private LastName() { }
}
