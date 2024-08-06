using Friendships.Read.Application.Abstractions.Messaging;

namespace Friendships.Read.Application.Commands.ChangeFriendshipType;

public sealed record ChangeFriendshipTypeCommand(
    Guid UserId,
    Guid FriendId,
    int NewFriendshipType) : ICommand;
