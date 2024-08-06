-- Получаване на типа на приключено приятелство
CREATE PROCEDURE [dbo].[GetEndedFriendshipType]
    @UserId UNIQUEIDENTIFIER,
    @FriendId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 [FriendshipType]
    FROM [dbo].[EndedFriendships]
    WHERE [UserId] = @UserId AND [FriendId] = @FriendId 
    UNION ALL
    SELECT TOP 1 [FriendshipType]
    FROM [dbo].[EndedFriendships]
    WHERE [UserId] = @FriendId AND [FriendId] = @UserId;

    IF @@ROWCOUNT = 0
        RETURN -1; -- Приключеното приятелство не съществува

    IF @@ROWCOUNT > 1
		RETURN -2; -- Открит е бъг. Има повече от едно приятелство между потребителите
END;