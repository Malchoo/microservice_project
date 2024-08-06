using Friendships.Write.Contracts.Friendships.Enums;

namespace Friendships.Write.Contracts.Friendships.ChangeRelationshipLevel;

public sealed record ChangeFriendshipLevelRequest(Guid FriendshipId, FriendshipLevel Currency);