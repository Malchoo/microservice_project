using ErrorOr;
using Friendships.Write.Contracts.Friendships.Create;
using Friendships.Write.Domain.Enums;
using MediatR;

namespace Friendships.Write.Application.Users.Create;

public record CreateFriendshipListCommand(
    Guid UserId,
    Guid InviterId,
    Guid InvitationId,
    FriendshipLevel FriendshipLevel) : IRequest<ErrorOr<CreateFriendshipResponse>>;