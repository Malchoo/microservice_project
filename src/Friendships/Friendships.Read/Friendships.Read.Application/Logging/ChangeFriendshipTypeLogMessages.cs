using Friendships.Read.Application.Commands.ChangeFriendshipType;
using Serilog;

namespace Friendships.Read.Application.Logging;

public static class ChangeFriendshipTypeLogMessages
{
    public static void LogSuccessfulChange(
        ILogger logger, 
        ChangeFriendshipTypeCommand command)
        => logger.Information("Friendship type changed successfully. " +
            "UserId: {UserId}, FriendId: {FriendId}, NewFriendshipType: {NewFriendshipType}",
            command.UserId, command.FriendId, command.NewFriendshipType);   

    public static void LogOneOrBothUsersDoNotExist(
        ILogger logger, 
        int errorCode, 
        Guid userId, 
        Guid friendId)
        => logger.Warning("Error code {ErrorCode}: One or both users do not exist. " +
            "User IDs are for user '{UserId}', amd for  friend is '{FriendId}'",
            errorCode, userId, friendId);

    public static void LogUnexpectedError(
        ILogger logger, 
        int errorCode, 
        ChangeFriendshipTypeCommand command, 
        Exception? exception = null)
        => logger.Error(exception, "Error code {ErrorCode}: Unexpected error occurred while changing " +
            "friendship type for user '{UserId}' and friend '{FriendId}'. " +
            "New friendship type is '{NewFriendshipType}', user Id is '{UserId}' '{FriendId}'.",
            errorCode, command.UserId, command.FriendId, command.NewFriendshipType);

    public static void LogUnhandledErrorCode(
        ILogger logger, 
        int errorCode, 
        ChangeFriendshipTypeCommand command, 
        Exception? exception = null)
        => logger.Error(exception, "Unhandled error code {ErrorCode} occurred while changing friendship" +
            " type for user '{UserId}' and friend '{FriendId}'. New friendship type is '{NewFriendshipType}'.",
            errorCode, command.UserId, command.FriendId, command.NewFriendshipType);
}
