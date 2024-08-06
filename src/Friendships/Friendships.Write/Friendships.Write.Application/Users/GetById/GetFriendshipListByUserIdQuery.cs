using ErrorOr;
using MediatR;

namespace Friendships.Write.Application.Users.GetById;

public sealed record GetFriendshipListByUserIdQuery(Guid UserId) : IRequest<ErrorOr<GetFriendshipListByUserIdResult>>;