-- Получаване на броя на приключените приятелства на потребител
CREATE PROCEDURE [dbo].[GetUserEndedFriendshipsCount]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [EndedFriendshipsCount]
    FROM [dbo].[UserSettings]
    WHERE [UserId] = @UserId;
END;