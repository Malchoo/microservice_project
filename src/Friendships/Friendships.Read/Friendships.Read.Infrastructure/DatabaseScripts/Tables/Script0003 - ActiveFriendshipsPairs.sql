CREATE TABLE [dbo].[ActiveFriendshipsPairs] (
    [SmallerUserId] INT NOT NULL,
    [LargerUserId] INT NOT NULL,
    [FriendshipId] UNIQUEIDENTIFIER NOT NULL,
    [FriendshipType] TINYINT NOT NULL,
    [InvitationId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATE NOT NULL,
    [LastModified] DATE NOT NULL,
    CONSTRAINT [PK_ActiveFriendshipsPairs] PRIMARY KEY CLUSTERED ([SmallerUserId], [LargerUserId])
);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendshipsPairs_FriendshipId]
ON [dbo].[ActiveFriendshipsPairs] ([FriendshipId]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendshipsPairs_FriendshipType] 
ON [dbo].[ActiveFriendshipsPairs] ([FriendshipType]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendshipsPairs_DateCreated] 
ON [dbo].[ActiveFriendshipsPairs] ([DateCreated]);

CREATE NONCLUSTERED INDEX [IX_ActiveFriendshipsPairs_LastModified] 
ON [dbo].[ActiveFriendshipsPairs] ([LastModified]);