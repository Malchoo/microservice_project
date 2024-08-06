using FluentValidation;
using System.Text.RegularExpressions;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class EmailValidator : AbstractValidator<Email>
{
    private readonly Regex _validCharactersRegex = new Regex(@"^[a-zA-Z0-9\._@-]+$");

    public EmailValidator()
    {
        RuleFor(email => email.Value)
            .NotEmpty()
            .WithErrorCode(EmailErrors.Empty.Code)
            .WithMessage(EmailErrors.Empty.Description)

            .MinimumLength(Email.MinLength)
            .WithErrorCode(EmailErrors.TooShort.Code)
            .WithMessage(EmailErrors.TooShort.Description)

            .MaximumLength(Email.MaxLength)
            .WithErrorCode(EmailErrors.TooLong.Code)
            .WithMessage(EmailErrors.TooLong.Description)

            .EmailAddress()
            .WithErrorCode(EmailErrors.InvalidFormat.Code)
            .WithMessage(EmailErrors.InvalidFormat.Description)

            .Matches(_validCharactersRegex)
            .WithErrorCode(EmailErrors.InvalidCharacters.Code)
            .WithMessage(EmailErrors.InvalidCharacters.Description);
    }
}
