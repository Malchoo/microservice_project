using ErrorOr;

namespace Users.Domain.Errors.Preferences;

public static class ThemeErrors
{
    public static readonly Error CannotConvertFromValue = Error.Validation(
            code: "ThemeErrors.CannotConvertFromValue",
            description: $"Cannot convert to a valid theme.");

    public static Error CannotChangeTheme(string currentTheme, string attemptedTheme)
        => Error.Conflict(
            code: "ThemeErrors.CannotChangeTheme",
            description: $"Cannot change theme from {currentTheme} to {attemptedTheme}.");
}
