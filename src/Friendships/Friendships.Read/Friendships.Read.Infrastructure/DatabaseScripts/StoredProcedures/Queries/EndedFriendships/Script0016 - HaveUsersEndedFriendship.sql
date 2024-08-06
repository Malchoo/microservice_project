-- Проверка дали двама потребителя имат приключено приятелство
CREATE PROCEDURE [dbo].[HaveUsersEndedFriendship]
    @UserId UNIQUEIDENTIFIER,
    @FriendId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM [dbo].[EndedFriendships]
        WHERE ([UserId] = @UserId AND [FriendId] = @FriendId) OR ([UserId] = @FriendId AND [FriendId] = @UserId)
    )
        RETURN 1; -- Потребителите имат приключено приятелство
    ELSE
        RETURN 0; -- Потребителите нямат приключено приятелство
END;