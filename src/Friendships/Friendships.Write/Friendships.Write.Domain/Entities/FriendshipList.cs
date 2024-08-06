using ErrorOr;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.DomainEvents;
using Friendships.Write.Domain.Enums;
using Friendships.Write.Domain.Ids;
using Friendships.Write.Domain.Primitives;
using Friendships.Write.Domain.Validators;
using Friendships.Write.Domain.ValueObjects;
using System.Collections.Generic;

namespace Friendships.Write.Domain.Entities;

public sealed class FriendshipList : Aggregate<UserId>
{
    public const int MaxFriendsCount = 1000;

    private FriendshipCollection _activeFriendships = FriendshipCollection.Empty(FriendshipState.Active);
    private FriendshipCollection _endedFriendships = FriendshipCollection.Empty(FriendshipState.Ended);
    private UniqueIdCollection<InvitationId> _invitationIds = UniqueIdCollection<InvitationId>.Empty<InvitationId>();
    private UniqueIdCollection<BlockedId> _blockedIds = UniqueIdCollection<BlockedId>.Empty<BlockedId>();
    private List<Error> _errors = new();

    private FriendshipList(UserId userId, Settings settings)
    {
        Id = userId;
        Settings = settings;
    }

    internal Settings Settings { get; private set; } = null!;

    public static ErrorOr<FriendshipList> Create(UserId userId)
    {
        var settings = Settings.Create();

        var userFriends = new FriendshipList(userId, settings);

        var validationResult = new FriendshipListValidator().Validate(userFriends);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(validationFailure => Error.Validation(
                    code: validationFailure.ErrorCode,
                    description: validationFailure.ErrorMessage))
                .ToList();

            return errors;
        }

        userFriends.RaiseDomainEvent(new FriendshipListCreatedDomainEvent(userFriends));
        return userFriends;
    }

    public ErrorOr<FriendshipList> CreateFriendship(InviterId inviterId, InvitationId invitationId)
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            CheckMaxFriendsCount();
            if (_errors.Count > 0)
                return _errors;

            ValidateForCreateon(inviterId, invitationId);
            if (_errors.Count > 0)
                return _errors;

            var friendId = new FriendId(inviterId.Value);
            var userId = new UserId(Id.Value);

            var friendshipResult = Friendship.Create(userId, friendId, invitationId);
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            var friendship = friendshipResult.Value;

            var addToActiveFriendshipsResult = _activeFriendships.Add(friendship);
            if (addToActiveFriendshipsResult.IsError)
                return addToActiveFriendshipsResult.Errors;

            var addInvitationIdResult = _invitationIds.Add(invitationId);
            if (addInvitationIdResult.IsError)
                return addInvitationIdResult.Errors;

            RaiseDomainEvent(new FriendshipCreatedDomainEvent(friendship));
            return this;
        });
    }

    public ErrorOr<FriendshipList> AcceptFriendship(Friendship friendship)
    {
        CheckMaxFriendsCount();

        if (_errors.Contains(Errors.Entities.FriendshipList.ReachedMaxFriendshipsCount(MaxFriendsCount)))
        {
            RaiseDomainEvent(new FriendshipRejectedDomainEvent(friendship));
            return _errors;
        }

        ValidationForАcceptance(friendship);
        if (_errors.Count > 0)
            return _errors;

        var friendshipResult = Friendship.Accept(friendship);
        if (friendshipResult.IsError)
            return friendshipResult.Errors;

        var addToActiveFriendshipsResult = _activeFriendships.Add(friendshipResult.Value);
        if(addToActiveFriendshipsResult.IsError)
            return addToActiveFriendshipsResult.Errors;

        var addInvitationIdResult = _invitationIds.Add(friendshipResult.Value.InvitationId);
        if (addInvitationIdResult.IsError)
            return addInvitationIdResult.Errors;

        RaiseDomainEvent(new FriendshipAcceptedDomainevent(friendshipResult.Value));
        return this;
    }

    public ErrorOr<FriendshipList> RejectFriendship(Friendship friendship)
    {
        //ToDo трябва още работа тук
        ValidateForRejection(friendship);
        if (_errors.Count > 0)
            return _errors;

        var removeFromActiveFriendhsipsResult = _activeFriendships.Remove(friendship);
        if (removeFromActiveFriendhsipsResult.IsError)
            return removeFromActiveFriendhsipsResult.Errors;

        return this;
    }

    public ErrorOr<FriendshipList> EndFriendship(FriendId friendId)
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            ValidateForEnding(friendId);
            if (_errors.Count > 0)
                return _errors;

            var friendshipResult = _activeFriendships.GetFriendshipByFriendshipId(friendId);
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            friendshipResult = friendshipResult.Value.EndFriendship();
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            var friendship = friendshipResult.Value;
            var result = MoveToEnded(friendship);
            if (result.IsError)
                return result.Errors;

            RaiseDomainEvent(new FriendshipEndedDomainEvent(friendship));
            return this;
        });
    }

    public ErrorOr<FriendshipList> ResotreFriendship(FriendId friendId)
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            ValideteForRestoring(friendId);
            if (_errors.Count > 0)
                return _errors;

            var friendshipResult = _activeFriendships.GetFriendshipByFriendshipId(friendId);
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            var friendship = friendshipResult.Value;
            friendshipResult = friendship.RestoreFriendship();
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            friendship = friendshipResult.Value;

            var result = MoveToActive(friendship);
            if (result.IsError)
                return result.Errors;

            RaiseDomainEvent(new FriendshipRestoredDomainEvent(friendship));
            return this;
        });
    }

    public ErrorOr<FriendshipList> PromoteFriendshipToTrusted(FriendId friendId)
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            ValidateToUpdateStatus(friendId);
            if (_errors.Count > 0)
                return _errors;

            var friendshipResult = _activeFriendships.GetFriendshipByFriendshipId(friendId);
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            friendshipResult = friendshipResult.Value.ToTrustedUser();
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            var friendship = friendshipResult.Value;
            RaiseDomainEvent(new FriendshipPromotedToTrustedDomainEvent(friendship));
            return this;
        });
    }

    public ErrorOr<FriendshipList> PromoteFriendshipToStandard(FriendId friendId)
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            ValidateToUpdateStatus(friendId);
            if (_errors.Count > 0)
                return _errors;

            var friendshipResult = _activeFriendships.GetFriendshipByFriendshipId(friendId);
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            friendshipResult = friendshipResult.Value.ToStandardFriend();
            if (friendshipResult.IsError)
                return friendshipResult.Errors;

            var friendship = friendshipResult.Value;

            RaiseDomainEvent(new FriendshipPromotedToStandardDomainEvent(friendship));
            return this;
        });
    }

    public ErrorOr<FriendshipList> DeleteFriendshipList()
    {
        return IfNotDeletedOrBlockedByAdmin(() =>
        {
            var deleteUserResult = Settings.DeleteFriendshipList();
            if (deleteUserResult.IsError)
                return deleteUserResult.Errors;

            RaiseDomainEvent(new DeleteAggregateDomainEvent(this));
            return this;
        });
    }

    public ErrorOr<FriendshipList> RestoreFriendshipList()
    {
        return IfNotBlockedByAdmin(() =>
        {
            var deleteUserResult = Settings.RestoreFriendshipList();
            if (deleteUserResult.IsError)
                return deleteUserResult.Errors;

            RaiseDomainEvent(new RestoreAggregateDomainEvent(this));
            return this;
        });
    }

    public ErrorOr<FriendshipList> BlockByAdmin()
    {
        var blockByAdminResult = Settings.BlockByAdmin();
        if (blockByAdminResult.IsError)
            return blockByAdminResult.Errors;

        RaiseDomainEvent(new UserBlockedByAdminDomainEvent(this));
        return this;
    }

    public ErrorOr<FriendshipList> UnblockByAdmin()
    {
        var blockByAdminResult = Settings.UnblockByAdmin();
        if (blockByAdminResult.IsError)
            return blockByAdminResult.Errors;

        RaiseDomainEvent(new UserUnblockedByAdminDomainEvent(this));
        return this;
    }


    public ErrorOr<FriendshipList> AddToBlockList(BlockedId blockedId)
    {
        if (IsUserBlocked(blockedId))
            return Errors.Entities.FriendshipList.AlreadyBlockedUser(blockedId);

        var addblockedIdResult = _blockedIds.Add(blockedId);
        if (addblockedIdResult.IsError)
            return addblockedIdResult.Errors;

        if (_activeFriendships.Contains(blockedId))
        {
            var friendship = _activeFriendships.GetFriendshipByFriendshipId(blockedId);
            var result = MoveToEnded(friendship.Value);
            if (result.IsError)
                return result.Errors;
        }

        RaiseDomainEvent(new UserBlockedDomainEvent(Id, blockedId)); //ToDo направи го IntegrationalEvent
        return this;
    }

    public ErrorOr<FriendshipList> RemoveFromBlockList(BlockedId blockedId)
    {
        if (!IsUserBlocked(blockedId))
            return Errors.Entities.FriendshipList.UserNotBlocked;

        var removeBlockedId = _blockedIds.Remove(blockedId);
        if (removeBlockedId.IsError)
            return removeBlockedId.Errors;

        RaiseDomainEvent(new UserUnblockedDomainEvent(Id, blockedId)); //ToDo направи го IntegrationalEvent
        return this;
    }

    private ErrorOr<FriendshipList> IfNotDeletedOrBlockedByAdmin(Func<ErrorOr<FriendshipList>> action)
    {
        return IfNotBlockedByAdmin(() =>
        {
            if (Settings.IsDeleted.Value)
                return Errors.Entities.FriendshipList.FriendsIsDeleted;

            return action();
        });
    }

    private ErrorOr<FriendshipList> IfNotBlockedByAdmin(Func<ErrorOr<FriendshipList>> action)
    {
        if (Settings.IsBlockedByAdmin.Value)
            return Errors.Entities.FriendshipList.BlockedByAdmin;

        return action();
    }

    private void ValidateForCreateon(InviterId inviterId, InvitationId invitationId)
    {
        ValidateForCreation(inviterId);

        ValidateForCreation(invitationId);
    }

    private void ValidateForCreation(InviterId inviterId)
    {
        if (_activeFriendships.Contains(inviterId))
            _errors.Add(Errors.Entities.FriendshipList.AlreadyFriend(inviterId));

        if (_endedFriendships.Contains(inviterId))
            _errors.Add(Errors.Entities.FriendshipList.AlreadyEndedFriendship(inviterId));
    }

    private void ValidateForCreation(InvitationId invitationId)
    {
        if (IsInvitationUsed(invitationId))
            _errors.Add(Errors.Entities.FriendshipList.InvitationAlreadyUsed(invitationId));
    }

    private void ValidationForАcceptance(Friendship friendship)
    {
        if (Id.Value != friendship.FriendId.Value)
            _errors.Add(Errors.Entities.FriendshipList.WrongRecipient(friendship.FriendId));

        if (IsInvitationUsed(friendship.InvitationId))
            _errors.Add(Errors.Entities.FriendshipList.CannotAccpetInvitationIdAlreadyUsed(friendship.InvitationId));

        if (FriendExist(friendship.UserId))
            _errors.Add(Errors.Entities.FriendshipList.CannotAcceptAlreadyFriend(friendship.FriendId));
    }

    private void ValidateForRejection(Friendship friendship)
    {
        //ToDo трябва да trow-на грешка тук?
        if (Id.Value != friendship.UserId.Value)
            _errors.Add(Errors.Entities.FriendshipList.CannotRejectWrongRecipient(friendship.Id, friendship.UserId));

        if (!IsInvitationUsed(friendship.InvitationId))
            _errors.Add(Errors.Entities.FriendshipList.CannotRejectInvitationIdMissMatch(friendship.InvitationId));
    }

    private void ValidateForEnding(FriendId friendId)
    {
        if (!FriendExist(friendId))
            _errors.Add(Errors.Entities.FriendshipList.NotFound(friendId));

        if (_endedFriendships.Contains(friendId))
            _errors.Add(Errors.Entities.FriendshipList.AlreadyEndedFriendship(friendId));
    }

    private void ValideteForRestoring(FriendId friendId)
    {
        if (!FriendExist(friendId))
            _errors.Add(Errors.Entities.FriendshipList.NotFound(friendId));

        if (_activeFriendships.Contains(friendId))
            _errors.Add(Errors.Entities.FriendshipList.AlreadyFriend(friendId));
    }

    public void ValidateToUpdateStatus(FriendId friendId)
    {
        if (!FriendExist(friendId))
            _errors.Add(Errors.Entities.FriendshipList.NotFound(friendId));

        if (_endedFriendships.Contains(friendId))
            _errors.Add(Errors.Entities.FriendshipList.AlreadyEndedFriendship(friendId));
    }

    private ErrorOr<Success> MoveToEnded(Friendship friendship)
    {
        var removeFromActiveFriendhsipsResult = _activeFriendships.Remove(friendship);
        if (removeFromActiveFriendhsipsResult.IsError)
            return removeFromActiveFriendhsipsResult.Errors;

        var addToEndedFriendshipsResult = _endedFriendships.Add(friendship);
        if (addToEndedFriendshipsResult.IsError)
            return addToEndedFriendshipsResult.Errors;

        return new Success();
    }

    private ErrorOr<Success> MoveToActive(Friendship friendship)
    {
        var removeFromActiveFriendhsipsResult = _endedFriendships.Remove(friendship);
        if (removeFromActiveFriendhsipsResult.IsError)
            return removeFromActiveFriendhsipsResult.Errors;

        var addToActiveFriendshipsResult = _activeFriendships.Add(friendship);
        if (addToActiveFriendshipsResult.IsError)
            return addToActiveFriendshipsResult.Errors;

        return new Success();
    }

    private bool IsUserBlocked(BlockedId blockedId)
        => _blockedIds.Contains(blockedId);

    private bool IsInvitationUsed(InvitationId invitationId)
        => _invitationIds.Contains(invitationId);

    private bool FriendExist(IEntityId id)
       => _activeFriendships.Contains(id) || _endedFriendships.Contains(id);

    private void CheckMaxFriendsCount()
    {
        if (_activeFriendships.Count > MaxFriendsCount)
        {
            RaiseDomainEvent(new ReachedMaxFriendsCountDomainEvent(this));

            _errors.Add(Errors.Entities.FriendshipList.ReachedMaxFriendshipsCount(MaxFriendsCount));
        }
    }

    private FriendshipList() //EF Core
    {
    }
}