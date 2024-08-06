CREATE PROCEDURE [dbo].[GetUserEndedFriendshipsByType]
    @UserId UNIQUEIDENTIFIER,
    @FriendshipType INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId], [FriendshipType], [InvitationId]
    FROM [dbo].[EndedFriendships]
    WHERE [UserId] = @UserId AND [FriendshipType] = @FriendshipType;
    UNION ALL
    SELECT [FriendId], [FriendshipType], [InvitationId]
    FROM [dbo].[EndedFriendships]
	WHERE [FriendId] = @UserId AND [FriendshipType] = @FriendshipType;
END;