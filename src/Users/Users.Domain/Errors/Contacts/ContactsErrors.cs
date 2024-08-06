using ErrorOr;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Errors.Contacts;

public static class ContactsErrors
{
    public static readonly Error Empty = Error.NotFound(
        code: "ContactsErrors.Empty",
        description: "User contact info is empty.");

    public static readonly Error EmailNotSet = Error.NotFound(
        code: "ContactsErrors.EmailNotSet",
        description: "Email is missing.");

    public static readonly Error MobileNumberNotSet = Error.NotFound(
        code: "ContactsErrors.MobileNumberNotSet",
        description: "Mobile number is missing.");

    public static readonly Error MissingContacts = Error.Validation(
        code: "ContactsErrors.MissingContacts",
        description: "At least one contact information (Email or Mobile Number) " +
        "must be provided.");

    public static Error MobileNumberNotChanged(MobileNumber mobileNumber) => Error.Validation(
        code: "ContactsErrors.MobileNumberNotChanged",
        description: "Not new mobile number provided. " +
        $"You have already provided this mobile number '{mobileNumber.Value}'.");

    public static Error EmailNotChanged(Email email) => Error.Validation(
        code: "ContactsErrors.EmailNotChanged",
        description: "Not new email provided. " +
        $"You have already provided this email '{email.Value}'.");
}
