-- Получаване на всички приключени приятелства на текущия потребител
CREATE PROCEDURE [dbo].[GetUserEndedFriendships]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId], [FriendshipType], [InvitationId]
    FROM [dbo].[EndedFriendships]
    WHERE [UserId] = @UserId;
    UNION ALL
    SELECT [FriendId], [FriendshipType], [InvitationId]
    FROM [dbo].[EndedFriendships]
    WHERE [FriendId] = @UserId;
END;