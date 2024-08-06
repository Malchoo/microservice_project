-- Получаване на броя на активните приятелства на потребител
CREATE PROCEDURE [dbo].[GetUserActiveFriendshipsCount]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ActiveFriendshipsCount]
    FROM [dbo].[UserSettings]
    WHERE [UserId] = @UserId;
END;
