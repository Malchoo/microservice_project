CREATE PROCEDURE [dbo].[EndActiveFriendship]
    @UserGuid UNIQUEIDENTIFIER,
    @FriendGuid UNIQUEIDENTIFIER,
    @ResultMessage NVARCHAR(MAX) OUTPUT,
    @IsSuccess BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @UserId INT, @FriendId INT;
    DECLARE @ProcedureName NVARCHAR(128) = OBJECT_NAME(@@PROCID);
    DECLARE @ExecutionTime DATETIME2 = SYSDATETIME();
    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
    DECLARE @AdditionalInfo NVARCHAR(4000);
    DECLARE @AffectedRows INT = 0;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    DECLARE @ErrorLine INT;
    DECLARE @FriendshipId UNIQUEIDENTIFIER;
    DECLARE @FriendshipType TINYINT;
    DECLARE @InvitationId UNIQUEIDENTIFIER;

    SET @UserId = dbo.InlineGuidToInt(@UserGuid);
    SET @FriendId = dbo.InlineGuidToInt(@FriendGuid);

    IF @UserId IS NULL OR @FriendId IS NULL
    BEGIN
        SET @AdditionalInfo = 'Невалиден или липсващ GUID за потребител или приятел.';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
            @UserGuid, @FriendGuid, NULL, NULL, 
            NULL, 50000, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
        );
        SET @IsSuccess = 0;
        RETURN;
    END

    -- Проверка за съществуващо приятелство в ActiveFriendships преди транзакцията
    IF NOT EXISTS (
        SELECT 1 
        FROM [dbo].[ActiveFriendships] WITH (NOLOCK)
        WHERE (UserId = @UserId AND FriendId = @FriendId) 
           OR (UserId = @FriendId AND FriendId = @UserId)
    )
    BEGIN
        SET @AdditionalInfo = 'Приятелството не съществува в ActiveFriendships и не може да бъде прекратено.';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
            @UserGuid, @FriendGuid, NULL, NULL, 
            NULL, 50001, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
        );
        SET @IsSuccess = 0;
        RETURN;
    END

    -- Проверка за съществуващо приятелство в ActiveFriendshipsPairs преди транзакцията
    IF NOT EXISTS (
        SELECT 1
        FROM [dbo].[ActiveFriendshipsPairs] WITH (NOLOCK)
        WHERE (SmallerUserId = CASE WHEN @UserId < @FriendId THEN @UserId ELSE @FriendId END
           AND LargerUserId = CASE WHEN @UserId > @FriendId THEN @UserId ELSE @FriendId END)
    )
    BEGIN
        SET @AdditionalInfo = 'Приятелството не съществува в ActiveFriendshipsPairs и не може да бъде прекратено.';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
            @UserGuid, @FriendGuid, NULL, NULL, 
            NULL, 50002, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
        );
        SET @IsSuccess = 0;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Изтриване от ActiveFriendships с ексклузивно заключване
        WITH FriendshipInfo AS (
            SELECT TOP 1 FriendshipId, FriendshipType, InvitationId
            FROM [dbo].[ActiveFriendships] WITH (XLOCK, ROWLOCK)
            WHERE (UserId = @UserId AND FriendId = @FriendId) 
               OR (UserId = @FriendId AND FriendId = @UserId)
        )
        DELETE FROM [dbo].[ActiveFriendships]
        OUTPUT 
            deleted.FriendshipId,
            deleted.FriendshipType,
            deleted.InvitationId,
            COUNT(*) OVER() AS DeletedCount
        INTO @DeletedFriendships(FriendshipId, FriendshipType, InvitationId, DeletedCount)
        FROM FriendshipInfo
        WHERE (UserId = @UserId AND FriendId = @FriendId)
           OR (UserId = @FriendId AND FriendId = @UserId);

        SELECT TOP 1 
            @FriendshipId = FriendshipId,
            @FriendshipType = FriendshipType,
            @InvitationId = InvitationId,
            @AffectedRows = DeletedCount
        FROM @DeletedFriendships;

        IF @AffectedRows <> 2
        BEGIN
            SET @AdditionalInfo = 'Несъответствие в данните за активното приятелство в ActiveFriendships.';
            SET @ResultMessage = dbo.GenerateResultJSON(
                @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
                @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
                @AffectedRows, 50010, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
            );
            SET @IsSuccess = 0;
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Изтриване от ActiveFriendshipsPairs с ексклузивно заключване
        DELETE FROM [dbo].[ActiveFriendshipsPairs] WITH (XLOCK, ROWLOCK)
        WHERE (SmallerUserId = CASE WHEN @UserId < @FriendId THEN @UserId ELSE @FriendId END
           AND LargerUserId = CASE WHEN @UserId > @FriendId THEN @UserId ELSE @FriendId END);

        IF @@ROWCOUNT <> 1
        BEGIN
            SET @AdditionalInfo = 'Несъответствие в данните за активното приятелство в ActiveFriendshipsPairs.';
            SET @ResultMessage = dbo.GenerateResultJSON(
                @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
                @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
                @AffectedRows, 50011, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
            );
            SET @IsSuccess = 0;
            ROLLBACK TRANSACTION;
            RETURN;
        END

        SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

        -- Вмъкване в EndedFriendships
        INSERT INTO [dbo].[EndedFriendships] 
            ([UserId], [FriendId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified])
        VALUES 
            (@UserId, @FriendId, @FriendshipId, @FriendshipType, @InvitationId, @CurrentDate, @CurrentDate),
            (@FriendId, @UserId, @FriendshipId, @FriendshipType, @InvitationId, @CurrentDate, @CurrentDate);

        SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

        -- Вмъкване в EndedFriendshipsPairs
        INSERT INTO [dbo].[EndedFriendshipsPairs]
            ([SmallerUserId], [LargerUserId], [FriendshipId], [FriendshipType], [InvitationId], [DateCreated], [LastModified])
        VALUES
            (CASE WHEN @UserId < @FriendId THEN @UserId ELSE @FriendId END,
             CASE WHEN @UserId > @FriendId THEN @UserId ELSE @FriendId END,
             @FriendshipId, @FriendshipType, @InvitationId, @CurrentDate, @CurrentDate);

        SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

        -- Обновяване на UserSettings с ексклузивно заключване
        UPDATE [dbo].[UserSettings] WITH (XLOCK, ROWLOCK)
        SET [ActiveFriendshipsCount] = [ActiveFriendshipsCount] - 1,
            [EndedFriendshipsCount] = [EndedFriendshipsCount] + 1,
            [LastModified] = @CurrentDate
        WHERE [UserId] IN (@UserId, @FriendId);

        SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

        COMMIT TRANSACTION;

        SET @AdditionalInfo = 'Активно приятелство успешно прекратено';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 1, @UserId, @FriendId, 
            @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
            @AffectedRows, NULL, NULL, NULL, NULL, NULL, @AdditionalInfo
        );
        SET @IsSuccess = 1;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        SELECT 
            @ErrorNumber = ERROR_NUMBER(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE(),
            @ErrorLine = ERROR_LINE(),
            @ErrorMessage = ERROR_MESSAGE();

        SET @AdditionalInfo = 'Грешка при прекратяване на активно приятелство';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
            @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
            NULL, @ErrorNumber, @ErrorSeverity, @ErrorState, 
            @ErrorLine, @ErrorMessage, @AdditionalInfo
        );
        SET @IsSuccess = 0;
    END CATCH
END;