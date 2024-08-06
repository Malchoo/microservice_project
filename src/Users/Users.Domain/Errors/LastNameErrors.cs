using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors;

public static class LastNameErrors
{
    public static Error Empty => Error.Validation(
        code: "LastNameErrors.LastNameEmpty",
        description: "Last name cannot be empty.");

    public static Error TooShort => Error.Validation(
        code: "LastNameErrors.LastNameTooShort",
        description: $"Last name cannot be shorter than {LastName.MinLength} characters.");

    public static Error TooLong => Error.Validation(
        code: "LastNameErrors.LastNameTooLong",
        description: $"Last name cannot be longer than {LastName.MaxLength} characters.");

    public static Error InvalidCharacters => Error.Validation(
        code: "LastNameErrors.InvalidCharacters",
        description: "Last name can only contain letters from a single alphabet, either Latin or Cyrillic.");

    public static Error MixedAlphabets => Error.Validation(
        code: "LastNameErrors.MixedAlphabets",
        description: "Last name must not mix alphabets. Use either Latin or Cyrillic characters consistently.");


}
