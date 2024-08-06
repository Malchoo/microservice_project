CREATE PROCEDURE [dbo].[GetActiveFriendshipByPairIds]
    @UserId UNIQUEIDENTIFIER,
    @FriendId UNIQUEIDENTIFIER

AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 [FriendId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified]
    FROM [dbo].[ActiveFriendships]
    WHERE [UserId] = @UserId 
    AND [FriendId] = @FriendId;
END;