using ErrorOr;
using Users.Domain.Enums;

namespace Users.Domain.Errors.Preferences;

public static class CurrencyErrors
{
    public static readonly Error CannotConvertFromName = Error.Validation(
        code: "CurrencyErrors.CannotConvertFromName",
        description: $"Cannot convert to a valid currency.");
}
