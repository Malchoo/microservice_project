CREATE TABLE dbo.UserIntToGuidMapping (
    LocalUserId INT NOT NULL,
    GlobalUserId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_UserIntToGuidMapping PRIMARY KEY CLUSTERED (LocalUserId),
    CONSTRAINT CK_UserIntToGuidMapping_LocalUserId CHECK (LocalUserId > 0)
);

CREATE NONCLUSTERED INDEX IX_UserIntToGuidMapping_GlobalUserId
ON dbo.UserIntToGuidMapping (GlobalUserId);