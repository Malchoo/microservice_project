CREATE PROCEDURE [dbo].[GetAllActiveFriendshipsByUserIdAndType]
    @UserId UNIQUEIDENTIFIER,
    @FriendshipType INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified]
    FROM [dbo].[ActiveFriendships]
    WHERE [UserId] = @UserId 
    AND [FriendshipType] = @FriendshipType;
END;