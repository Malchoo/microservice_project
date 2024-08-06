using ErrorOr;
using Users.Domain.Errors.Contacts;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record Contacts
{
    private Contacts(Email email, MobileNumber mobileNumber)
    {
        Email = email;
        MobileNumber = mobileNumber;
    }

    public Email Email { get; init; } = null!;
    public MobileNumber MobileNumber { get; init; } = null!;

    internal static ErrorOr<Contacts> Create(Email email, MobileNumber mobileNumber)
    {
        var contacts = new Contacts(email, mobileNumber);

        var contactsResult = new ContactsValidator().Validate(contacts);

        if (!contactsResult.IsValid)
        {
            var errors = contactsResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        return contacts;
    }

    internal ErrorOr<Contacts> ChangeEmail(Email newEmail)
        => ChangeProperty(
            Email,
            newEmail,
            ContactsErrors.EmailNotChanged,
            email => this with { Email = email });

    internal ErrorOr<Contacts> ChangeMobileNumber(MobileNumber newMobileNumber)
        => ChangeProperty(
            MobileNumber,
            newMobileNumber,
            ContactsErrors.MobileNumberNotChanged,
            mobileNumber => this with { MobileNumber = mobileNumber });

    private ErrorOr<Contacts> ChangeProperty<T>(
            T currentValue,
            T newValue,
            Func<T, Error> errorFunc,
            Func<T, Contacts> changeFunc)
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            return errorFunc(newValue);

        return changeFunc(newValue);
    }

    private Contacts()
    {
    }
}
