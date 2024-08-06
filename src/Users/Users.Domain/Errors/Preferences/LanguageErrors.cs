using ErrorOr;

namespace Users.Domain.Errors.Preferences;

public static class LanguageErrors
{
    public static readonly Error CannotConvertFromValue = Error.Validation(
            code: "LanguageErrors.CannotConvertFromValue",
            description: $"Cannot convert to a valid language.");

    public static Error CannotChangeLanguage(string currentLanguage, string attemptedLanguage)
        => Error.Conflict(
            code: "LanguageErrors.CannotChangeLanguage",
            description: $"Cannot change language from {currentLanguage} to {attemptedLanguage}.");
}
