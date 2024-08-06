using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Profile;

public static class MiddleNameErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "MiddleNameErrors.MiddleNameEmpty",
        description: "Middle name cannot be empty.");

    public static readonly Error MiddleNameTooShort = Error.Validation(
        code: "MiddleNameErrors.MiddleNameTooShort",
        description: $"Middle name cannot be shorter than {MiddleName.MinLength} characters.");

    public static readonly Error MiddleNameTooLong = Error.Validation(
        code: "MiddleNameErrors.MiddleNameTooLong",
        description: $"Middle name cannot be longer than {MiddleName.MaxLength} characters.");
}