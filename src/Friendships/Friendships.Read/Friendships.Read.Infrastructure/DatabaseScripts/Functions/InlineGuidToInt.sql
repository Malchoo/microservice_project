CREATE FUNCTION dbo.InlineGuidToInt
(
    @GlobalUserId UNIQUEIDENTIFIER
)
RETURNS INT
WITH RETURNS NULL ON NULL INPUT,
     SCHEMABINDING
AS
BEGIN
    DECLARE @LocalUserId INT;

    SELECT @LocalUserId = LocalUserId
    FROM dbo.UserGuidToIntMapping
    WHERE GlobalUserId = @GlobalUserId;

    RETURN @LocalUserId;
END;