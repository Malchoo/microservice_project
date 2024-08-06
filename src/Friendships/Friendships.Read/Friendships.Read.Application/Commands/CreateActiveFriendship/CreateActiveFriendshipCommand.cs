using Friendships.Read.Application.Abstractions.Messaging;

namespace Friendships.Read.Application.Commands.CreateActiveFriendship;

public sealed record CreateActiveFriendshipCommand(
    Guid UserId,
    Guid FriendId,
    int FriendshipType,
    Guid InvitationId) : ICommand;
