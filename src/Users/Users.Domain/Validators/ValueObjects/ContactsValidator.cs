using FluentValidation;
using Users.Domain.Errors.Contacts;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class ContactsValidator : AbstractValidator<Contacts>
{
    public ContactsValidator()
    {
        //RuleFor(contacts => contacts.Id)
        //    .NotNull()
        //    .WithErrorCode(ContactsErrors.MissingUserID.Code)
        //    .WithMessage(ContactsErrors.MissingUserID.Description);

        //RuleFor(contacts => contacts)
        //    .Must(AtLeastOneContactProvided)
        //    .WithErrorCode(ContactsErrors.MissingContacts.Code)
        //    .WithMessage(ContactsErrors.MissingContacts.Description);
    }

    //private bool AtLeastOneContactProvided(Contacts contacts)
    //{
    //    return contacts.Email != null || contacts.MobileNumber != null;
    //}
}