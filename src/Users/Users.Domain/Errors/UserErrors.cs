using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound = Error.NotFound(
        code: "UserErrors.UserNotFound",
        description: "User not found.");

    public static readonly Error MissingUser = Error.NotFound(
        code: "UserErrors.MissingUser",
        description: "User is missing.");

    public static readonly Error MissingContacts = Error.NotFound(
        code: "UserErrors.MissingContacts",
        description: "User contacts information is missing.");

    public static readonly Error MissingSettings = Error.NotFound(
        code: "UserErrors.MissingSettings",
        description: "User settings are missing.");

    public static readonly Error MissingPreferences = Error.NotFound(
        code: "UserErrors.MissingPreferences",
        description: "User preferences are missing.");

    public static Error EmailIsNotSet = Error.Conflict(
        code: "UserErrors.EmailIsNotSet",
        description: "Email is not set.");

    public static Error MobileNumberIsNotSet = Error.Conflict(
        code: "UserErrors.MobileNumberIsNotSet",
        description: "Mobile number is not set.");

    public static readonly Error BlockedByAdmin = Error.Conflict(
       code: "UserErrors.BlockedByAdmin",
       description: "You cannot do the action. You are blocked by admin.");

    public static Error MobileNumberDoesNotMatch(MobileNumber mobileNumber) => Error.Conflict(
        code: "UserErrors.MobileNumberDoesNotMatch",
        description: $"Mobile number '{mobileNumber.Value}' does not match.");

    public static Error EmailDoesNotMatch(Email email) => Error.Conflict(
        code: "UserErrors.EmailDoesNotMatch",
        description: $"Email '{email.Value}'does not match.");

    public static Error EmailHasBeenTaken(Email email) => Error.Conflict(
        code: "UserErrors.EmailHasBeenTaken",
        description: $"The specified email '{email.Value}' has been taken.");

    public static Error MobileNumberHasBeenTaken(MobileNumber mobileNumber) => Error.Conflict(
        code: "UserErrors.MobileNumberHasBeenTaken",
        description: $"The specified mobile number '{mobileNumber.Value}' has been taken.");

    public static Error UsernameHasBeenTaken(Username username) => Error.Conflict(
        code: "UserErrors.UsernameHasBeenTaken",
        description: $"The specified username '{username.Value}' has been taken.");
}
