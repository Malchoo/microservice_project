using ErrorOr;
using MediatR;
using Users.Contracts.Users.Create;
using Users.Domain.Enums;

namespace Users.Application.Users.Create;

public record CreateUserCommand(
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
    Theme Theme) : IRequest<ErrorOr<CreateUserResponse>>;