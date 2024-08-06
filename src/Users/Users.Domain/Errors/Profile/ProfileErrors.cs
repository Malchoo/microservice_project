using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Profile;

public static class ProfileErrors
{
    public static readonly Error NotFoundOrAlreadyDeleted = Error.NotFound(
        code: "ProfileErrors.NotFoundOrAlreadyDeleted",
        description: "User not found or already deleted.");

    public static readonly Error MissingUsername = Error.NotFound(
        code: "ProfileErrors.MissingUsername",
        description: "Username is missing.");

    public static readonly Error MissingFullName = Error.NotFound(
        code: "ProfileErrors.MissingFullName",
        description: "Full name is missing.");

    public static Error UsernameNotChanged(Username username) => Error.Validation(
        code: "ProfileErrors.UsernameNotChanged",
        description: "Not new username provided. " +
        $"You have already provided this '{username.Value}' username.");

    public static Error FirstNameNotChanged(FirstName firstName) => Error.Validation(
        code: "ProfileErrors.FirstNameNotChanged",
        description: "Not new first name provided. " +
        $"You have already provided this '{firstName.Value}' first name.");

    public static Error MiddleNameNotChanged(MiddleName middleName) => Error.Validation(
        code: "ProfileErrors.MiddleNameNotChanged",
        description: "Not new middle name provided. " +
        $"You have already provided this '{middleName.Value}' middle name.");

    public static Error LastNameNotChanged(LastName lastName) => Error.Validation(
        code: "ProfileErrors.LastNameNotChanged",
        description: "Not new last name provided. " +
        $"You have already provided this '{lastName.Value}' last name.");
}
