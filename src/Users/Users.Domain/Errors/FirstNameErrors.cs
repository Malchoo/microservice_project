using ErrorOr;

namespace Users.Domain.Errors;

public static class FirstNameErrors
{
    public static Error Empty => Error.Validation(
        code: "FirstNameErrors.Empty",
        description: "First name cannot be empty.");

    public static Error TooShort(int minLength) => Error.Validation(
        code: "FirstNameErrors.TooShort",
        description: $"First name must be at least {minLength} characters long.");

    public static Error TooLong(int maxLength) => Error.Validation(
        code: "FirstNameErrors.TooLong",
        description: $"First name cannot be longer than {maxLength} characters.");

    public static Error InvalidCharacters => Error.Validation(
        code: "FirstNameErrors.InvalidCharacters",
        description: "First name can only contain letters from a single alphabet, either Latin or Cyrillic.");

    public static Error MixedAlphabets => Error.Validation(
        code: "FirstNameErrors.MixedAlphabets",
        description: "First name must not mix alphabets. Use either Latin or Cyrillic characters consistently.");

}
