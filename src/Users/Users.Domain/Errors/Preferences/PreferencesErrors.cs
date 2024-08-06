using ErrorOr;
using Users.Domain.Enums;

namespace Users.Domain.Errors.Preferences;

public static class PreferencesErrors
{
    public static readonly Error MissingTransAuthenticationMethod = Error.NotFound(
        code: "PreferencesErrors.MissingTransAuthenticationMethod",
        description: "Transaction Authentication Method enumeration is missing.");

    public static readonly Error MissingLanguage = Error.NotFound(
        code: "PreferencesErrors.MissingLanguage",
        description: "Language enumeration is missing.");

    public static readonly Error MissingTheme = Error.NotFound(
        code: "PreferencesErrors.MissingTheme",
        description: "Theme enumeration is missing.");

    public static Error CyrrencyNotChanged(Currency currency) => Error.Conflict(
        code: "PreferencesErrors.CurrencyNotChanged",
        description: "Not new currency provided. " +
        $"You have already provided this currency '{currency.Name}'.");

    public static Error TwoFactorAuthNotChanged(TwoFactorAuth twoFactorAuth) => Error.Conflict(
        code: "PreferencesErrors.TwoFactorAuthNotChanged",
        description: "Not new two factor authentication provided. " +
        $"You have already provided this two factor authentication '{twoFactorAuth.Name}'.");

    public static Error TitleNotChanged(Title title) => Error.Conflict(
        code: "PreferencesErrors.TitleNotChanged",
        description: "Not new title provided. " +
        $"You have already provided this title '{title.Name}'.");

    public static Error LanguageNotChanged(Language language) => Error.Conflict(
        code: "PreferencesErrors.LanguageNotChanged",
        description: "Not new language provided. " +
        $"You have already provided this language '{language.Name}'.");

    public static Error ThemeNotChanged(Theme theme) => Error.Conflict(
        code: "PreferencesErrors.ThemeNotChanged",
        description: "Not new theme provided. " +
        $"You have already provided this theme '{theme.Name}'.");
}
