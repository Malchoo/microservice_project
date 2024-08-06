-- Получаване на всички приятелства на текущия потребител
CREATE PROCEDURE [dbo].[GetAllActiveFriendshipsByUserId]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified]
    FROM [dbo].[ActiveFriendships]
    WHERE [UserId] = @UserId;
END;