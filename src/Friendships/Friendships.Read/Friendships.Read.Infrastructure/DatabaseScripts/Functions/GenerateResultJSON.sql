CREATE FUNCTION dbo.GenerateResultJSON
(
    @ProcedureName NVARCHAR(128),
    @ExecutionTime DATETIME2,
    @IsSuccess BIT,
    @UserId INT,
    @FriendId INT,
    @UserGuid UNIQUEIDENTIFIER,
    @FriendGuid UNIQUEIDENTIFIER,
    @FriendshipId UNIQUEIDENTIFIER = NULL,
    @FriendshipType TINYINT = NULL,
    @AffectedRows INT = NULL,
    @ErrorNumber INT = NULL,
    @ErrorSeverity INT = NULL,
    @ErrorState INT = NULL,
    @ErrorLine INT = NULL,
    @ErrorMessage NVARCHAR(4000) = NULL,
    @AdditionalInfo NVARCHAR(4000) = NULL
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @OperationId UNIQUEIDENTIFIER = NEWID();
    DECLARE @EndTime DATETIME2 = SYSDATETIME();
    DECLARE @Duration BIGINT = DATEDIFF(MICROSECOND, @ExecutionTime, @EndTime);

    RETURN (
        SELECT 
            '1.0' AS SchemaVersion,
            @OperationId AS OperationId,
            @ProcedureName AS [Procedure],
            @ExecutionTime AS StartTime,
            @EndTime AS EndTime,
            @Duration AS DurationMicroseconds,
            @IsSuccess AS IsSuccess,
            @UserId AS UserId,
            @FriendId AS FriendId,
            @UserGuid AS UserGuid,
            @FriendGuid AS FriendGuid,
            @FriendshipId AS FriendshipId,
            @FriendshipType AS FriendshipType,
            @AffectedRows AS AffectedRows,
            CASE WHEN @IsSuccess = 0 THEN
                (SELECT 
                    @ErrorNumber AS Number,
                    @ErrorSeverity AS Severity,
                    @ErrorState AS [State],
                    @ErrorLine AS Line,
                    @ErrorMessage AS [Message]
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
            ELSE NULL END AS Error,
            @AdditionalInfo AS AdditionalInfo
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );
END