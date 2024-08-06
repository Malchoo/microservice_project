using ErrorOr;

using MediatR;

namespace Users.Application.Profiles.CreateTrainerProfile;

public record CreateTrainerProfileCommand(Guid UserId)
    : IRequest<ErrorOr<Guid>>;
