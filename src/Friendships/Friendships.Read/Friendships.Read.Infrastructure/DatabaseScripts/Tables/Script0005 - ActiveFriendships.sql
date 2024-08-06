CREATE TABLE [dbo].[ActiveFriendships] (
    [UserId] INT NOT NULL,
    [FriendId] INT NOT NULL,
    [FriendshipId] UNIQUEIDENTIFIER NOT NULL,
    [FriendshipType] TINYINT NOT NULL,
    [InvitationId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATE NOT NULL,
    [LastModified] DATE NOT NULL,
    CONSTRAINT [PK_ActiveFriendships] PRIMARY KEY CLUSTERED ([UserId])
);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendships_FriendId] 
ON [dbo].[ActiveFriendships] ([FriendId]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendships_FriendshipId]
ON [dbo].[ActiveFriendships] ([FriendshipId]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendships_FriendshipType] 
ON [dbo].[ActiveFriendships] ([FriendshipType]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendships_DateCreated] 
ON [dbo].[ActiveFriendships] ([DateCreated]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendships_LastModified] 
ON [dbo].[ActiveFriendships] ([LastModified]);