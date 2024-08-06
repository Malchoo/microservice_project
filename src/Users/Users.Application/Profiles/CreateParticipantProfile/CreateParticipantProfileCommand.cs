using ErrorOr;

using MediatR;

namespace Users.Application.Profiles.CreateParticipantProfile;

public record CreateParticipantProfileCommand(Guid UserId)
    : IRequest<ErrorOr<Guid>>;
