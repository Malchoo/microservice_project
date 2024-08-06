using ErrorOr;
using Users.Domain.Enums;
using Users.Domain.Errors.Preferences;
using Users.Domain.Validators.ValueObjects;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record Preferences
{
    private Preferences(
        Currency currency,
        TwoFactorAuth twoFactorAuth,
        Title title,
        Language language,
        Theme theme)
    {
        Currency = currency;
        TwoFactorAuth = twoFactorAuth;
        Title = title;
        Language = language;
        Theme = theme;
    }

    public Currency Currency { get; init; } = null!;
    public TwoFactorAuth TwoFactorAuth { get; init; } = null!;
    public Title Title { get; init; } = null!;
    public Language Language { get; init; } = null!;
    public Theme Theme { get; init; } = null!;

    internal static ErrorOr<Preferences> Create(
        Currency? currency,
        TwoFactorAuth? twoFactorChannel,
        Title? title,
        Language? language,
        Theme? theme)
    {
        var preferences = new Preferences(
            currency ?? Currency.BGN,
            twoFactorChannel ?? TwoFactorAuth.SMS,
            title ?? Title.None,
            language ?? Language.EN,
            theme ?? Theme.Light);

        var preferencesResult = new PreferencesValidator().Validate(preferences);
        if (!preferencesResult.IsValid)
        {
            var errors = preferencesResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        return preferences;
    }

    internal ErrorOr<Preferences> ChangeCurrency(Currency newCurrency)
        => ChangeProperty(
            Currency,
            newCurrency,
            PreferencesErrors.CyrrencyNotChanged,
            currency => this with { Currency = currency }
            );

    internal ErrorOr<Preferences> ChangeTwoFactorAuth(TwoFactorAuth twoFactorAuth)
        => ChangeProperty(
            TwoFactorAuth,
            twoFactorAuth,
            PreferencesErrors.TwoFactorAuthNotChanged,
            newValue => this with { TwoFactorAuth = newValue });

    internal ErrorOr<Preferences> ChangeTitle(Title title)
        => ChangeProperty(
            Title,
            title,
            PreferencesErrors.TitleNotChanged,
            newValue => this with { Title = newValue });

    internal ErrorOr<Preferences> ChangeLanguage(Language language)
        => ChangeProperty(
            Language,
            language,
            PreferencesErrors.LanguageNotChanged,
            newValue => this with { Language = newValue });

    internal ErrorOr<Preferences> ChangeTheme(Theme theme)
        => ChangeProperty(
            Theme,
            theme,
            PreferencesErrors.ThemeNotChanged,
            newValue => this with { Theme = newValue });

    private static ErrorOr<Preferences> ChangeProperty<T>(
           T currentValue,
           T newValue,
           Func<T, Error> errorFunc,
           Func<T, Preferences> changeFunc)
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            return errorFunc(newValue);

        return changeFunc(newValue);
    }

    private Preferences()
    {
    }
}