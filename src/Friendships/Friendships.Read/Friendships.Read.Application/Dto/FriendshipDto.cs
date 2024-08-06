using Friendships.Read.Application.Enums;

namespace Friendships.Read.Application.Dto;

public sealed record FriendshipDto(
    Guid UserId, 
    Guid FriendId, 
    FriendshipType FriendshipType, 
    Guid InvitationId);