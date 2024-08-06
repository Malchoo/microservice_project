CREATE PROCEDURE [dbo].[ChangeFriendshipType]
    @UserGuid UNIQUEIDENTIFIER,
    @FriendGuid UNIQUEIDENTIFIER,
    @NewFriendshipType TINYINT,
    @ResultMessage NVARCHAR(MAX) OUTPUT,
    @IsSuccess BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;

    DECLARE @UserId INT, @FriendId INT;
    DECLARE @SmallerUserId INT, @LargerUserId INT;
    
    SELECT @UserId = LocalUserId FROM dbo.InlineGuidToInt(@UserGuid);
    SELECT @FriendId = LocalUserId FROM dbo.InlineGuidToInt(@FriendGuid);
    
    -- Проверка дали GUID-овете са валидни
    IF @UserId IS NULL OR @FriendId IS NULL
    BEGIN
        SET @ErrorMessage = 'Невалиден GUID за потребител или приятел.';
        RETURN;
    END

    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
    DECLARE @ExecutionTime DATETIME2 = SYSDATETIME();
    DECLARE @RowsAffected INT;
    DECLARE @OldFriendshipType TINYINT;
    DECLARE @FriendshipId UNIQUEIDENTIFIER;
    DECLARE @ProcedureName NVARCHAR(128) = OBJECT_NAME(@@PROCID);
        
    -- Определяне на по-малкото и по-голямото UserId
    IF @UserId < @FriendId
    BEGIN
        SET @SmallerUserId = @UserId;
        SET @LargerUserId = @FriendId;
    END
    ELSE
    BEGIN
        SET @SmallerUserId = @FriendId;
        SET @LargerUserId = @UserId;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Проверка и актуализация в ActiveFriendshipsPairs
        UPDATE [dbo].[ActiveFriendshipsPairs]
        SET 
            @OldFriendshipType = [FriendshipType],
            @FriendshipId = [FriendshipId],
            [FriendshipType] = @NewFriendshipType,
            [LastModified] = @CurrentDate
        WHERE [SmallerUserId] = @SmallerUserId AND [LargerUserId] = @LargerUserId;

        SET @RowsAffected = @@ROWCOUNT;

        IF @RowsAffected = 0
        BEGIN
            SET @ResultMessage = CONCAT('ChangeFriendshipType: Активно приятелство не е намерено в таблицата ActiveFriendshipsPairs. ',
                                        'UserId: ', @UserId, ', FriendId: ', @FriendId, 
                                        ', SmallerUserId: ', @SmallerUserId, ', LargerUserId: ', @LargerUserId,
                                        ', Време на изпълнение: ', @ExecutionTime);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Актуализация в ActiveFriendships
        UPDATE [dbo].[ActiveFriendships]
        SET 
            [FriendshipType] = @NewFriendshipType,
            [LastModified] = @CurrentDate
        WHERE ([UserId] = @UserId AND [FriendId] = @FriendId)
           OR ([UserId] = @FriendId AND [FriendId] = @UserId);

        IF @@ROWCOUNT <> 2
        BEGIN
            SET @ResultMessage = CONCAT('ChangeFriendshipType: Несъответствие в данните за активно приятелство в таблицата ActiveFriendships. ',
                                        'UserId: ', @UserId, ', FriendId: ', @FriendId, 
                                        ', SmallerUserId: ', @SmallerUserId, ', LargerUserId: ', @LargerUserId,
                                        ', Очаквани редове: 2, Актуализирани редове: ', @@ROWCOUNT,
                                        ', Време на изпълнение: ', @ExecutionTime);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        COMMIT TRANSACTION;
        SET @ErrorMessage = NULL; -- Успех, няма грешка
    END TRY

    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        SET @ResultMessage = (
            SELECT 
                @ProcedureName AS [Procedure],
                ERROR_NUMBER() AS ErrorNumber,
                ERROR_SEVERITY() AS ErrorSeverity,
                ERROR_STATE() AS ErrorState,
                ERROR_LINE() AS ErrorLine,
                ERROR_MESSAGE() AS ErrorMessage,
                @ExecutionTime AS ExecutionTime,
                @UserId AS UserId,
                @FriendId AS FriendId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
    END CATCH
END;