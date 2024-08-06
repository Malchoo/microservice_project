CREATE PROCEDURE [dbo].[GetActiveFriendshipTypeByPairIds]
    @UserId INT,
    @FriendId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP 1 [FriendshipType]
    FROM [dbo].[ActiveFriendshipsPairs]
    WHERE [UserId] = @UserId AND [FriendId] = @FriendId

    IF @@ROWCOUNT = 0
    BEGIN
		SELECT TOP 1 [FriendshipType]
		FROM [dbo].[ActiveFriendshipsPairs]
		WHERE [UserId] = @FriendId AND [FriendId] = @UserId
	END
END;