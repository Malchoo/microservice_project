using Friendships.Write.Contracts.Friendships.Enums;

namespace Friendships.Write.Contracts.Friendships.Create;

public sealed record CreateFriendshipRequest(
    Guid UserId,
    Guid FriendId,
    Guid InvitationId,
    FriendshipLevel FriendshipLevel);