using FluentValidation;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Entities;

namespace Friendships.Write.Domain.Validators;

public class FriendshipListValidator : AbstractValidator<FriendshipList>
{
    public FriendshipListValidator()
    {
        RuleFor(aggregate => aggregate.Id)
            .Must(id => id.Value != Guid.Empty)
            .WithErrorCode(Errors.Entities.FriendshipList.InvalidUserId.Code)
            .WithMessage(Errors.Entities.FriendshipList.InvalidUserId.Description);

        RuleFor(aggregate => aggregate.Settings.IsDeleted)
            .Must(isDeleted => isDeleted.Value == false)
            .WithErrorCode(Errors.Entities.FriendshipList.CannotBeDeletedWhenCreating.Code)
            .WithMessage(Errors.Entities.FriendshipList.CannotBeDeletedWhenCreating.Description);

        RuleFor(aggregate => aggregate.Settings.IsBlockedByAdmin)
            .Must(isBlockedByAdmin => isBlockedByAdmin.Value == false)
            .WithErrorCode(Errors.Entities.FriendshipList.CannotBeBlockedByAdminWhenCreating.Code)
            .WithMessage(Errors.Entities.FriendshipList.CannotBeBlockedByAdminWhenCreating.Description);
    }
}
