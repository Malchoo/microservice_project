CREATE PROCEDURE [dbo].[GetActiveFriendshipByInvitation]
    @InvitationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 [FriendId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified]
    FROM [dbo].[ActiveFriendships]
    WHERE [InvitationId] = @InvitationId;
END;
