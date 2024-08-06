using FluentValidation;
using System.Text.RegularExpressions;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class MiddleNameValidator : AbstractValidator<MiddleName>
{
    private readonly Regex _LatinAlphabetRegex = new Regex(@"^[a-zA-Z]+$");
    private readonly Regex _CyrillicAlphabetRegex = new Regex(@"^[а-яА-Я]+$");

    public MiddleNameValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithErrorCode(MiddleNameErrors.Empty.Code)
            .WithMessage(MiddleNameErrors.Empty.Description)

            .MinimumLength(MiddleName.MinLength)
            .WithErrorCode(MiddleNameErrors.TooShort(MiddleName.MinLength).Code)
            .WithMessage(MiddleNameErrors.TooShort(MiddleName.MinLength).Description)

            .MaximumLength(MiddleName.MaxLength)
            .WithErrorCode(MiddleNameErrors.TooLong(MiddleName.MaxLength).Code)
            .WithMessage(MiddleNameErrors.TooLong(MiddleName.MaxLength).Description)

            .Must(BeAlphabeticallyConsistent)
            .WithErrorCode(MiddleNameErrors.InvalidCharacters.Code)
            .WithMessage(MiddleNameErrors.InvalidCharacters.Description)

            .Must(BeSingleAlphabet)
            .WithErrorCode(MiddleNameErrors.MixedAlphabets.Code)
            .WithMessage(MiddleNameErrors.MixedAlphabets.Description);
    }

    private bool BeAlphabeticallyConsistent(string name)
    {
        return _LatinAlphabetRegex.IsMatch(name) || _CyrillicAlphabetRegex.IsMatch(name);
    }

    private bool BeSingleAlphabet(string name)
    {
        bool isLatin = _LatinAlphabetRegex.IsMatch(name);
        bool isCyrillic = _CyrillicAlphabetRegex.IsMatch(name);

        return isLatin != isCyrillic;
    }
}
