using FluentValidation;
using System.Text.RegularExpressions;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public sealed class UsernameValidator : AbstractValidator<Username>
{
    private readonly Regex _allowedCharactersRegex = new Regex(@"^[a-zA-Zа-яА-Я0-9]+$");

    public UsernameValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithErrorCode(UsernameErrors.Empty.Code)
            .WithMessage(UsernameErrors.Empty.Description)

            .MinimumLength(Username.MinLength)
            .WithErrorCode(UsernameErrors.TooShort(Username.MinLength).Code)
            .WithMessage(UsernameErrors.TooShort(Username.MinLength).Description)

            .MaximumLength(Username.MaxLength)
            .WithErrorCode(UsernameErrors.TooLong(Username.MaxLength).Code)
            .WithMessage(UsernameErrors.TooLong(Username.MaxLength).Description)

            .Must(BeAlphabeticallyConsistent)
            .WithErrorCode(UsernameErrors.InvalidCharacters.Code)
            .WithMessage(UsernameErrors.InvalidCharacters.Description)

            .Must(BeSingleAlphabet)
            .WithErrorCode(UsernameErrors.MixedAlphabets.Code)
            .WithMessage(UsernameErrors.MixedAlphabets.Description)

            .Matches(_allowedCharactersRegex)
            .WithErrorCode(UsernameErrors.Invalid.Code)
            .WithMessage(UsernameErrors.Invalid.Description);
    }

    private bool BeAlphabeticallyConsistent(string name)
    {
        return _allowedCharactersRegex.IsMatch(name);
    }

    private bool BeSingleAlphabet(string name)
    {
        bool containsLatin = name.Any(ch => ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || char.IsDigit(ch));
        bool containsCyrillic = name.Any(ch => ch >= 'а' && ch <= 'я' || ch >= 'А' && ch <= 'Я');

        // Ако има и латински, и кирилица, върнете false
        return !(containsLatin && containsCyrillic);
    }

}
