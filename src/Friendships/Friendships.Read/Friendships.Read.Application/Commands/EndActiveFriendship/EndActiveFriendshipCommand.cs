using Friendships.Read.Application.Abstractions.Messaging;

namespace Friendships.Read.Application.Commands.EndActiveFriendship;

public sealed record EndActiveFriendshipCommand(
    Guid UserId,
    Guid FriendId) : ICommand;