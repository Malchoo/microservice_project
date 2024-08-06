using Friendships.Read.Application.Abstractions.Messaging;

namespace Friendships.Read.Application.Commands.RestoreEndedFriendship;

public sealed record RestoreEndedFriendshipCommand(
    Guid UserId,
    Guid FriendId) : ICommand;