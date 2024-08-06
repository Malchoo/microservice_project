CREATE OR ALTER PROCEDURE [dbo].[EndActiveFriendship]
    @UserGuid UNIQUEIDENTIFIER,
    @FriendGuid UNIQUEIDENTIFIER,
    @ResultMessage NVARCHAR(MAX) OUTPUT,
    @IsSuccess BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;

    DECLARE @UserId INT, @FriendId INT;
    DECLARE @ProcedureName NVARCHAR(128) = OBJECT_NAME(@@PROCID);
    DECLARE @ExecutionTime DATETIME2 = SYSDATETIME();
    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
    DECLARE @AdditionalInfo NVARCHAR(4000);
    DECLARE @AffectedRows INT = 0;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorNumber INT, @ErrorSeverity INT, @ErrorState INT, @ErrorLine INT;
    DECLARE @FriendshipId UNIQUEIDENTIFIER, @FriendshipType TINYINT, @InvitationId UNIQUEIDENTIFIER;
    DECLARE @MaxAttempts INT = 5;
    DECLARE @Attempts INT = 0;
    DECLARE @RetryDelay DATETIME = DATEADD(MILLISECOND, 100, 0);

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

    WHILE @Attempts < @MaxAttempts
    BEGIN
        SET @Attempts = @Attempts + 1;

        BEGIN TRY
            BEGIN TRANSACTION;

            -- Проверка и изтриване от ActiveFriendships с UPDLOCK
            WITH FriendshipInfo AS (
                SELECT TOP 1 FriendshipId, FriendshipType, InvitationId
                FROM [dbo].[ActiveFriendships] WITH (UPDLOCK, HOLDLOCK)
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

            IF NOT EXISTS (SELECT 1 FROM @DeletedFriendships)
            BEGIN
                SET @AdditionalInfo = 'Приятелството не съществува или вече е прекратено.';
                SET @ResultMessage = dbo.GenerateResultJSON(
                    @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
                    @UserGuid, @FriendGuid, NULL, NULL, 
                    0, 50001, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
                );
                SET @IsSuccess = 0;
                ROLLBACK TRANSACTION;
                RETURN;
            END

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

            -- Изтриване от ActiveFriendshipsPairs с UPDLOCK
            DELETE FROM [dbo].[ActiveFriendshipsPairs] WITH (UPDLOCK, HOLDLOCK)
            WHERE (SmallerUserId = CASE WHEN @UserId < @FriendId THEN @UserId ELSE @FriendId END
               AND LargerUserId = CASE WHEN @UserId > @FriendId THEN @UserId ELSE @FriendId END);

            SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

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

            -- Обновяване на UserSettings с UPDLOCK
            UPDATE [dbo].[UserSettings] WITH (UPDLOCK, HOLDLOCK)
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
            RETURN;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 
                ROLLBACK TRANSACTION;

            IF ERROR_NUMBER() = 1205 -- Deadlock error
            BEGIN
                SET @AdditionalInfo = CONCAT('Опит ', @Attempts, ' от ', @MaxAttempts, ' неуспешен поради deadlock. Повторен опит след ', DATEDIFF(MILLISECOND, 0, @RetryDelay), ' ms.');
                WAITFOR DELAY @RetryDelay;
                SET @RetryDelay = DATEADD(MILLISECOND, 100, @RetryDelay);
                CONTINUE;
            END
            ELSE
            BEGIN
                SELECT 
                    @ErrorNumber = ERROR_NUMBER(),
                    @ErrorSeverity = ERROR_SEVERITY(),
                    @ErrorState = ERROR_STATE(),
                    @ErrorLine = ERROR_LINE(),
                    @ErrorMessage = ERROR_MESSAGE();

                SET @AdditionalInfo = CONCAT('Грешка при прекратяване на активно приятелство. Опит ', @Attempts, ' от ', @MaxAttempts);
                SET @ResultMessage = dbo.GenerateResultJSON(
                    @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
                    @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
                    NULL, @ErrorNumber, @ErrorSeverity, @ErrorState, 
                    @ErrorLine, @ErrorMessage, @AdditionalInfo
                );
                SET @IsSuccess = 0;
                RETURN;
            END
        END CATCH
    END

    -- Ако достигнем тук, значи всички опити са били неуспешни
    SET @AdditionalInfo = CONCAT('Неуспешно прекратяване на активно приятелство след ', @MaxAttempts, ' опита.');
    SET @ResultMessage = dbo.GenerateResultJSON(
        @ProcedureName, @ExecutionTime, 0, @UserId, @FriendId, 
        @UserGuid, @FriendGuid, @FriendshipId, @FriendshipType, 
        NULL, 50012, 16, 1, NULL, 'Максимален брой опити достигнат', @AdditionalInfo
    );
    SET @IsSuccess = 0;
END;