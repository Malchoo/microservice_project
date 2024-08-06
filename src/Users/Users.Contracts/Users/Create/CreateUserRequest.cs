using Users.Contracts.Users.Enums;

namespace Users.Contracts.Users.Create;

public record CreateUserRequest(
    Guid RegistrationId,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string MobileNumber,
    Currency Currency,
    TwoFactorAuth TwoFactorAuth,
    Title Title,
    Language Language,
    Theme Theme);