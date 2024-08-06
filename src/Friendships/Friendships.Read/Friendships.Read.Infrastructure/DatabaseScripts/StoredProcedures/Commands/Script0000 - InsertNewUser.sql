CREATE OR ALTER PROCEDURE [dbo].[InsertNewUser]
    @GlobalUserId UNIQUEIDENTIFIER,
    @MaxAttempts INT = 5,
    @ResultMessage NVARCHAR(MAX) OUTPUT,
    @IsSuccess BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;

    DECLARE @LocalUserId INT;
    DECLARE @ProcedureName NVARCHAR(128) = OBJECT_NAME(@@PROCID);
    DECLARE @ExecutionTime DATETIME2 = SYSDATETIME();
    DECLARE @AdditionalInfo NVARCHAR(4000);
    DECLARE @AffectedRows INT = 0;
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;
    DECLARE @ErrorLine INT;
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @Attempts INT = 0;
    DECLARE @RetryLog NVARCHAR(1000) = '';

    IF EXISTS (
        SELECT 1 
        FROM dbo.UserGuidToIntMapping WITH (NOLOCK) 
        WHERE GlobalUserId = @GlobalUserId)
    BEGIN
        SET @AdditionalInfo = 'GUID вече съществува в базата данни.';
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, NULL, NULL, 
            @GlobalUserId, NULL, NULL, NULL, 
            NULL, 50000, 16, 1, NULL, @AdditionalInfo, @AdditionalInfo
        );
        SET @IsSuccess = 0;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        WHILE @Attempts < @MaxAttempts
        BEGIN
            SET @Attempts = @Attempts + 1;
            
            SET @LocalUserId = NEXT VALUE FOR dbo.LocalUserIdSequence;

            IF NOT EXISTS (
                SELECT 1 
                FROM dbo.UserIntToGuidMapping WITH (UPDLOCK, HOLDLOCK) 
                WHERE LocalUserId = @LocalUserId)
            BEGIN
                INSERT INTO dbo.UserGuidToIntMapping (GlobalUserId, LocalUserId)
                VALUES (@GlobalUserId, @LocalUserId);
                
                SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

                INSERT INTO dbo.UserIntToGuidMapping (LocalUserId, GlobalUserId)
                VALUES (@LocalUserId, @GlobalUserId);

                SET @AffectedRows = @AffectedRows + @@ROWCOUNT;

                COMMIT TRANSACTION;

                IF @Attempts > 1
                BEGIN
                    SET @AdditionalInfo = 'Нов потребител успешно добавен след ' + CAST(@Attempts AS NVARCHAR(10)) + ' опита. ' + @RetryLog;
                END
                ELSE
                BEGIN
                    SET @AdditionalInfo = 'Нов потребител успешно добавен.';
                END

                SET @ResultMessage = dbo.GenerateResultJSON(
                    @ProcedureName, @ExecutionTime, 1, @LocalUserId, NULL, 
                    @GlobalUserId, NULL, NULL, NULL, 
                    @AffectedRows, NULL, NULL, NULL, NULL, NULL, @AdditionalInfo
                );
                SET @IsSuccess = 1;
                RETURN;
            END
            ELSE
            BEGIN
                SET @RetryLog = @RetryLog + 'Опит ' + CAST(@Attempts AS NVARCHAR(10)) + ': LocalUserId ' + CAST(@LocalUserId AS NVARCHAR(20)) + ' вече съществува. ';
            END
        END

        -- Ако достигнем тук, значи сме изчерпали всички опити
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        SET @AdditionalInfo = 'Достигнат е максималният брой опити (' + CAST(@MaxAttempts AS NVARCHAR(10)) + ') за генериране на уникален LocalUserId. ' + @RetryLog;
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, NULL, NULL, 
            @GlobalUserId, NULL, NULL, NULL, 
            NULL, 50002, 16, 1, NULL, 'Неуспешно генериране на уникален LocalUserId', @AdditionalInfo
        );
        SET @IsSuccess = 0;
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

        SET @AdditionalInfo = 'Грешка при добавяне на нов потребител. ' + @RetryLog;
        SET @ResultMessage = dbo.GenerateResultJSON(
            @ProcedureName, @ExecutionTime, 0, NULL, NULL, 
            @GlobalUserId, NULL, NULL, NULL, 
            NULL, @ErrorNumber, @ErrorSeverity, @ErrorState, 
            @ErrorLine, @ErrorMessage, @AdditionalInfo
        );
        SET @IsSuccess = 0;
    END CATCH
END;