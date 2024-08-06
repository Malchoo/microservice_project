using Friendships.Read.Application.Commands.EndActiveFriendship;
using Serilog;

namespace Friendships.Read.Application.Logging;

public static class EndActiveFriendshipLogMessages
{
    public static void LogSuccessfulEndActiveFriendship(
        ILogger logger, 
        EndActiveFriendshipCommand command)
        => logger.Information("Active friendship ended successfully. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            command.UserId, command.FriendId);

    public static void LogFriendshipNotFound(
        ILogger logger, 
        int errorCode, 
        Guid userId, 
        Guid friendId)
        => logger.Warning("Error code {ErrorCode}: Active friendship not found. User IDs: {UserId}, {FriendId}",
            errorCode, userId, friendId);

    public static void LogUnexpectedError(
        ILogger logger,
        int errorCode,
        EndActiveFriendshipCommand command,
        Exception? exception = null)
        => logger.Error(exception, "Error code {ErrorCode}: Unexpected error occurred while ending active friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            errorCode, command.UserId, command.FriendId);

    public static void LogUnhandledErrorCode(
        ILogger logger,
        int errorCode,
        EndActiveFriendshipCommand command,
        Exception? exception = null)
        => logger.Error(exception, "Unhandled error code {ErrorCode} occurred while ending active friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            errorCode, command.UserId, command.FriendId);
}
