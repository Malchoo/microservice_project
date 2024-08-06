CREATE TABLE [dbo].[EndedFriendships] (
    [UserId] INT NOT NULL,
    [FriendId] INT NOT NULL,
    [FriendshipId] UNIQUEIDENTIFIER NOT NULL,
    [FriendshipType] TINYINT NOT NULL,
    [InvitationId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATE NOT NULL,
    [LastModified] DATE NOT NULL,
    CONSTRAINT [PK_EndedFriendships] PRIMARY KEY CLUSTERED ([UserId])
);

CREATE NONCLUSTERED INDEX [IX_EndedFriendships_FriendId] 
ON [dbo].[EndedFriendships] ([FriendId]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendships_FriendshipId]
ON [dbo].[EndedFriendships] ([FriendshipId]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendships_FriendshipType] 
ON [dbo].[EndedFriendships] ([FriendshipType]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendships_DateCreated] 
ON [dbo].[EndedFriendships] ([DateCreated]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendships_LastModified] 
ON [dbo].[EndedFriendships] ([LastModified]);