CREATE TABLE [dbo].[EndedFriendshipsPairs] (
    [SmallerUserId] INT NOT NULL,
    [LargerUserId] INT NOT NULL,
    [FriendshipId] UNIQUEIDENTIFIER NOT NULL,
    [FriendshipType] TINYINT NOT NULL,
    [InvitationId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATE NOT NULL,
    [LastModified] DATE NOT NULL,
    CONSTRAINT [PK_EndedFriendshipsPairs] PRIMARY KEY CLUSTERED ([SmallerUserId], [LargerUserId])
);

CREATE NONCLUSTERED INDEX [IX_EndedFriendshipsPairs_FriendshipId]
ON [dbo].[EndedFriendshipsPairs] ([FriendshipId]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendshipsPairs_FriendshipType] 
ON [dbo].[EndedFriendshipsPairs] ([FriendshipType]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendshipsPairs_DateCreated] 
ON [dbo].[EndedFriendshipsPairs] ([DateCreated]);

CREATE NONCLUSTERED INDEX [IX_EndedFriendshipsPairs_LastModified] 
ON [dbo].[EndedFriendshipsPairs] ([LastModified]);