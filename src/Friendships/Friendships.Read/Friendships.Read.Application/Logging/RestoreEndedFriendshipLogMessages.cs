using Friendships.Read.Application.Commands.RestoreEndedFriendship;
using Serilog;

namespace Friendships.Read.Application.Logging;

public static class RestoreEndedFriendshipLogMessages
{
    public static void LogSuccessfulRestoreEndedFriendship(
        ILogger logger, 
        RestoreEndedFriendshipCommand command)
        => logger.Information("Ended friendship restored successfully. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            command.UserId, command.FriendId);

    public static void LogFriendshipNotFound(
        ILogger logger,
        int errorCode,
        Guid userId,
        Guid friendId)
        => logger.Warning("Error code {ErrorCode}: Ended friendship not found. User IDs: {UserId}, {FriendId}",
            errorCode, userId, friendId);

    public static void LogUnexpectedError(
        ILogger logger,
        int errorCode,
        RestoreEndedFriendshipCommand command,
        Exception? exception = null)
        => logger.Error(exception, "Error code {ErrorCode}: Unexpected error occurred while restoring ended friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            errorCode, command.UserId, command.FriendId);

    public static void LogUnhandledErrorCode(
        ILogger logger,
        int errorCode,
        RestoreEndedFriendshipCommand command,
        Exception? exception = null)
        => logger.Error(exception, "Unhandled error code {ErrorCode} occurred while restoring ended friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}",
            errorCode, command.UserId, command.FriendId);
}
