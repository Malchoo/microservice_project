-- Получаване на списък с идентификатори на приключените приятели на потребител
CREATE PROCEDURE [dbo].[GetUserEndedFriendIds]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [FriendId]
    FROM [dbo].[EndedFriendships]
    WHERE [UserId] = @UserId;
END;