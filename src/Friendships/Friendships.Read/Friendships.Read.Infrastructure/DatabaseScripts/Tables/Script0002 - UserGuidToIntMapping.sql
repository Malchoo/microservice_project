CREATE TABLE dbo.UserGuidToIntMapping (
    GlobalUserId UNIQUEIDENTIFIER NOT NULL,
    LocalUserId INT NOT NULL,
    CONSTRAINT PK_UserGuidToIntMapping PRIMARY KEY CLUSTERED (GlobalUserId),
    CONSTRAINT CK_UserGuidToIntMapping_LocalUserId CHECK (LocalUserId > 0),
    CONSTRAINT UQ_UserGuidToIntMapping_LocalUserId UNIQUE (LocalUserId)
);

CREATE NONCLUSTERED INDEX IX_UserGuidToIntMapping_LocalUserId
ON dbo.UserGuidToIntMapping (LocalUserId);