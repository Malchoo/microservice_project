using ErrorOr;

namespace Users.Domain.Errors;

public static class MiddleNameErrors
{
    public static Error Empty => Error.Validation(
        code: "MiddleNameErrors.Empty",
        description: "Middle name cannot be empty.");

    public static Error TooShort(int minLength) => Error.Validation(
        code: "MiddleNameErrors.TooShort",
        description: $"Middle name must be at least {minLength} characters long.");

    public static Error TooLong(int maxLength) => Error.Validation(
        code: "MiddleNameErrors.TooLong",
        description: $"Middle name cannot be longer than {maxLength} characters.");

    public static Error InvalidCharacters => Error.Validation(
        code: "MiddleNameErrors.InvalidCharacters",
        description: "Middle name can only contain letters from a single alphabet, either Latin or Cyrillic.");

    public static Error MixedAlphabets => Error.Validation(
        code: "MiddleNameErrors.MixedAlphabets",
        description: "Middle name must not mix alphabets. Use either Latin or Cyrillic characters consistently.");
}
