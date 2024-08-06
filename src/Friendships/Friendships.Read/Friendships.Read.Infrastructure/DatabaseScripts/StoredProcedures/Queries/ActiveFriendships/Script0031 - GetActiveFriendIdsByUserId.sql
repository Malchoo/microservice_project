-- Получаване на списък с идентификатори на активните приятели на потребител
CREATE PROCEDURE [dbo].[GetUserActiveFriendIds]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId]
    FROM [dbo].[ActiveFriendships]
    WHERE [UserId] = @UserId;
    UNION ALL
    SELECT [UserId]
    FROM [dbo].[ActiveFriendships]
	WHERE [FriendId] = @UserId;
END;