using ErrorOr;

using MediatR;

namespace Users.Application.Profiles.ListProfiles;

public record ListProfilesQuery(Guid UserId) : IRequest<ErrorOr<ListProfilesResult>>;