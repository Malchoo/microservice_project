using ErrorOr;

namespace Users.Domain.Errors;

public static class UsernameErrors
{
    public static Error Empty => Error.Validation(
        code: "UsernameErrors.Empty",
        description: "Username is empty.");

    public static Error TooShort(int minLength) => Error.Validation(
        code: "UsernameErrors.TooShort",
        description: $"Username must be at least {minLength} characters long.");

    public static Error TooLong(int maxLength) => Error.Validation(
        code: "UsernameErrors.TooLong",
        description: $"Username must bErre less than {maxLength} characters long.");

    public static Error NotFound(Guid registration) => Error.NotFound(
        code: "UsernameErrors.NotFound",
        description: $"Username with id '{registration}' was not found.");

    public static Error NotFound(string value) => Error.NotFound(
        code: "UsernameErrors.NotFound",
        description: $"Username with input parameter of {value} was not found.");

    //public static Error NotFound(Email email) => Error.NotFound(
    //    code: "UsernameErrors.NotFound",
    //    description: $"Username with email '{email.Value}' was not found.");

    //public static Error NotFound(MobileNumber mobileNumber) => Error.NotFound(
    //    code: "UsernameErrors.NotFound",
    //    description: $"Username with mobile number '{mobileNumber.Value}' was not found.");

    public static Error Invalid => Error.Validation(
        code: "UsernameErrors.Invalid",
        description: "Username must only contain letters.");

    public static Error NoWhiteSpaces => Error.Validation(
        code: "UsernameErrors.NoWhiteSpace",
        description: "Username must not contain spaces.");

    public static Error AlreadyExists => Error.Conflict(
        code: "UsernameErrors.AlreadyExists",
        description: "This username already exists.");

    public static Error IsBlockedByAdmin => Error.Conflict(
        code: "UsernameErrors.IsBlocked",
        description: "This username is blocked.");

    public static Error InvalidCharacters => Error.Validation(
       code: "UsernameErrors.InvalidCharacters",
       description: "Username can only contain letters, numbers, and allowed symbols from a single alphabet.");

    public static Error MixedAlphabets => Error.Validation(
        code: "UsernameErrors.MixedAlphabets",
        description: "Username must not mix alphabets. Use either Latin or Cyrillic characters consistently.");
}
