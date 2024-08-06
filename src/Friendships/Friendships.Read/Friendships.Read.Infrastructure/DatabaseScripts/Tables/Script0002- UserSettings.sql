CREATE TABLE [dbo].[UserSettings] (
    [UserId] INT NOT NULL PRIMARY KEY,
    [IsBlockedByAdmin] BIT NOT NULL DEFAULT 0,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [ActiveFriendshipsCount] INT NOT NULL DEFAULT 0,
    [EndedFriendshipsCount] INT NOT NULL DEFAULT 0,
    [LastModified] DATE NOT NULL
);

CREATE NONCLUSTERED INDEX [IX_UserSettings_IsBlockedByAdmin]
ON [dbo].[UserSettings] ([IsBlockedByAdmin]);

CREATE NONCLUSTERED INDEX [IX_UserSettings_IsDeleted]
ON [dbo].[UserSettings] ([IsDeleted]);

CREATE NONCLUSTERED INDEX [IX_UserSettings_ActiveFriendshipsCount]
ON [dbo].[UserSettings] ([ActiveFriendshipsCount]);

CREATE NONCLUSTERED INDEX [IX_UserSettings_EndedFriendshipsCount]
ON [dbo].[UserSettings] ([EndedFriendshipsCount]);

CREATE NONCLUSTERED INDEX [IX_UserSettings_LastModified]
ON [dbo].[UserSettings] ([LastModified]);