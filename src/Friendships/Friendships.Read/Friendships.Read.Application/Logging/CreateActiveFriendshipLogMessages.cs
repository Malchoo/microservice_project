using Friendships.Read.Application.Commands.CreateActiveFriendship;
using Serilog;

namespace Friendships.Read.Application.Logging;

public static class CreateActiveFriendshipLogMessages
{
    public static void LogSuccessfulCreation(
        ILogger logger,
        CreateActiveFriendshipCommand command)
        => logger.Information("Active friendship created successfully. " +
            "UserId: {UserId}, FriendId: {FriendId}, FriendshipType: {FriendshipType}, InvitationId: {InvitationId}",
            command.UserId, command.FriendId, command.FriendshipType, command.InvitationId);

    public static void LogOneOrBothUsersDoNotExist(
        ILogger logger,
        int errorCode,
        Guid userId,
        Guid friendId)
        => logger.Warning("Error code {ErrorCode}: One or both users do not exist. User IDs: {UserId}, {FriendId}",
            errorCode, userId, friendId);

    public static void LogFriendshipAlreadyExists(
        ILogger logger, 
        int errorCode, 
        Guid userId, 
        Guid friendId)
        => logger.Information("Error code {ErrorCode}: Friendship already exists. User IDs: {UserId}, {FriendId}",
            errorCode, userId, friendId);

    public static void LogUnexpectedError(
        ILogger logger, 
        int errorCode, 
        CreateActiveFriendshipCommand command, 
        Exception? exception = null)
        => logger.Error(exception, "Error code {ErrorCode}: Unexpected error occurred while creating active friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}, FriendshipType: {FriendshipType}, InvitationId: {InvitationId}",
            errorCode, command.UserId, command.FriendId, command.FriendshipType, command.InvitationId);

    public static void LogUnhandledErrorCode(
        ILogger logger, 
        int errorCode, 
        CreateActiveFriendshipCommand command)
        => logger.Error("Unhandled error code {ErrorCode} occurred while creating active friendship. " +
            "UserId: {UserId}, FriendId: {FriendId}, FriendshipType: {FriendshipType}, InvitationId: {InvitationId}",
            errorCode, command.UserId, command.FriendId, command.FriendshipType, command.InvitationId);    
}