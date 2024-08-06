using Friendships.Write.Contracts.Friendships.Enums;

namespace Friendships.Write.Contracts.Friendships.GetByFriendshipId;

public sealed record GetByFriendshipIdResponse(
    Guid UserId,
    Guid FriendUserId,
    Guid InvitationId,
    FriendshipLevel FriendshipLevel,
    IsDeleted IsDeleted);
