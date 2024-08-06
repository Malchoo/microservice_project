using FluentValidation;
using System.Text.RegularExpressions;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class LastNameValidator : AbstractValidator<LastName>
{
    private readonly Regex _LatinAlphabetRegex = new Regex(@"^[a-zA-Z]+$");
    private readonly Regex _CyrillicAlphabetRegex = new Regex(@"^[а-яА-Я]+$");

    public LastNameValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithErrorCode(LastNameErrors.Empty.Code)
            .WithMessage(LastNameErrors.Empty.Description)

            .MinimumLength(LastName.MinLength)
            .WithErrorCode(LastNameErrors.TooShort.Code)
            .WithMessage(LastNameErrors.TooShort.Description)

            .MaximumLength(LastName.MaxLength)
            .WithErrorCode(LastNameErrors.TooLong.Code)
            .WithMessage(LastNameErrors.TooLong.Description)

            .Matches(_LatinAlphabetRegex)
            .WithErrorCode(LastNameErrors.InvalidCharacters.Code)
            .WithMessage(LastNameErrors.InvalidCharacters.Description)

            .Must(BeSingleAlphabet)
            .WithErrorCode(LastNameErrors.MixedAlphabets.Code)
            .WithMessage(LastNameErrors.MixedAlphabets.Description);
    }

    private bool BeAlphabeticallyConsistent(string name)
    {
        return _LatinAlphabetRegex.IsMatch(name) || _CyrillicAlphabetRegex.IsMatch(name);
    }

    private bool BeSingleAlphabet(string name)
    {
        // Проверява дали всички символи са или само латиница, или само кирилица.
        bool isLatin = _LatinAlphabetRegex.IsMatch(name);
        bool isCyrillic = _CyrillicAlphabetRegex.IsMatch(name);

        // Ако има символи от и двете азбуки, връщаме false.
        return isLatin != isCyrillic;
    }
}
