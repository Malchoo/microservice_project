using ErrorOr;

namespace Users.Api.Errors;

public static class UserControllerErrors
{
    public static Error CannotConvertEnum(string enumName) => Error.Unexpected(
        code: "UserControllerErrors.EnumCannotConvert",
        description: $"Invalid enum value for {enumName}. This value cannot convert.");
}