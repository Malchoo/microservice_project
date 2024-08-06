CREATE PROCEDURE [dbo].[AreUsersFriends]
    @UserId UNIQUEIDENTIFIER,
    @FriendId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверка дали съществува приятелство в едната посока
    IF EXISTS (
        SELECT 1 
        FROM [dbo].[ActiveFriendships]
        WHERE ([UserId] = @UserId AND [FriendId] = @FriendId)
    )
    BEGIN
        RETURN 1; -- Потребителите са приятели
    END

    -- Проверка дали съществува приятелство в обратната посока
    IF EXISTS (
        SELECT 1 
        FROM [dbo].[ActiveFriendships]
        WHERE ([UserId] = @FriendId AND [FriendId] = @UserId)
    )
    BEGIN
        RETURN 1; -- Потребителите са приятели
    END

    RETURN 0; -- Потребителите не са приятели
END
