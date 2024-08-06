using FluentValidation;
using System.Text.RegularExpressions;
using Users.Domain.Errors;
using Users.Domain.ValueObjects.ReferenceType;

namespace Users.Domain.Validators.ValueObjects;

public class FirstNameValidator : AbstractValidator<FirstName>
{
    private readonly Regex _LatinAlphabetRegex = new(@"^[a-zA-Z]+$");
    private readonly Regex _CyrillicAlphabetRegex = new(@"^[а-яА-Я]+$");

    public FirstNameValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithErrorCode(FirstNameErrors.Empty.Code)
            .WithMessage(FirstNameErrors.Empty.Description);

        RuleFor(x => x.Value)
            .MinimumLength(FirstName.MinLength)
            .WithErrorCode(FirstNameErrors.TooShort(FirstName.MinLength).Code)
            .WithMessage(FirstNameErrors.TooShort(FirstName.MinLength).Description);

        RuleFor(x => x.Value)
            .MaximumLength(FirstName.MaxLength)
            .WithErrorCode(FirstNameErrors.TooLong(FirstName.MaxLength).Code)
            .WithMessage(FirstNameErrors.TooLong(FirstName.MaxLength).Description);

        RuleFor(x => x.Value)
            .Must(BeAlphabeticallyConsistent)
            .WithErrorCode(FirstNameErrors.InvalidCharacters.Code)
            .WithMessage(FirstNameErrors.InvalidCharacters.Description);

        RuleFor(x => x.Value)
            .Must(BeSingleAlphabet)
            .WithErrorCode(FirstNameErrors.MixedAlphabets.Code)
            .WithMessage(FirstNameErrors.MixedAlphabets.Description);
    }

    private bool BeAlphabeticallyConsistent(string name)
    {
        return _LatinAlphabetRegex.IsMatch(name) || _CyrillicAlphabetRegex.IsMatch(name);
    }

    private bool BeSingleAlphabet(string name)
    {
        if (_LatinAlphabetRegex.IsMatch(name))
        {
            return !_CyrillicAlphabetRegex.IsMatch(name);
        }
        return true;
    }
}
