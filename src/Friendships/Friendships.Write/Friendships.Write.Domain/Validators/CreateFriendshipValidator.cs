using FluentValidation;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Enums;

namespace Friendships.Write.Domain.Validators;

public class CreateFriendshipValidator : AbstractValidator<Friendship>
{
    public CreateFriendshipValidator()
    {
        RuleFor(friendship => friendship.Id)
            .Must(id => id.Value != Guid.Empty)
            .WithErrorCode(Errors.Entities.Friendship.InvalidId.Code)
            .WithMessage(Errors.Entities.Friendship.InvalidId.Description);

        RuleFor(friendship => friendship.UserId)
           .Must(id => id.Value != Guid.Empty)
           .WithErrorCode(Errors.Entities.Friendship.InvalidUserId.Code)
           .WithMessage(Errors.Entities.Friendship.InvalidUserId.Description);

        RuleFor(friendship => friendship.FriendId)
            .Must(id => id.Value != Guid.Empty)
            .WithErrorCode(Errors.Entities.Friendship.InvalidFriendId.Code)
            .WithMessage(Errors.Entities.Friendship.InvalidFriendId.Description);

        RuleFor(friendship => friendship.InvitationId)
            .Must(invId => invId.Value != Guid.Empty)
            .WithErrorCode(Errors.Entities.Friendship.InvalidInvitationId.Code)
            .WithMessage(Errors.Entities.Friendship.InvalidInvitationId.Description);

        RuleFor(friendship => friendship)
            .Must(friendship => friendship.UserId.Value != friendship.FriendId.Value)
            .WithErrorCode(Errors.Entities.Friendship.SelfFriendship.Code)
            .WithMessage(Errors.Entities.Friendship.SelfFriendship.Description);

        RuleFor(friendship => friendship.Level)
            .NotEmpty()
            .WithErrorCode(Errors.Entities.Friendship.LevelCannotBeEmpty.Code)
            .WithMessage(Errors.Entities.Friendship.LevelCannotBeEmpty.Description);

        RuleFor(friendship => friendship.Level)
            .Equal(FriendshipLevel.Standard)
            .WithErrorCode(Errors.Entities.Friendship.InvalidCreationLevel.Code)
            .WithMessage(Errors.Entities.Friendship.InvalidCreationLevel.Description);

        RuleFor(friendship => friendship.State)
            .NotEmpty()
            .WithErrorCode(Errors.Entities.Friendship.StatusCannotBeEmpty.Code)
            .WithMessage(Errors.Entities.Friendship.StatusCannotBeEmpty.Description);

        RuleFor(friendship => friendship.State)
            .Equal(FriendshipState.Active)
            .WithErrorCode(Errors.Entities.Friendship.InvalidStatus.Code)
            .WithMessage(Errors.Entities.Friendship.InvalidStatus.Description);
    }
}