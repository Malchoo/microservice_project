using ErrorOr;
using Friendships.Domain.DomainErrors;

namespace Friendships.Write.Domain.ValueObjects;

public sealed record Settings
{
    private Settings(
        IsBlockedByAdmin isBlockedByAdmin,
        IsDeleted isDeleted)
    {
        IsBlockedByAdmin = isBlockedByAdmin;
        IsDeleted = isDeleted;
    }

    public IsBlockedByAdmin IsBlockedByAdmin { get; init; }
    public IsDeleted IsDeleted { get; init; }

    internal static Settings Create()
    {
        var isBlockedByAdmin = IsBlockedByAdmin.No;

        var isDeleted = IsDeleted.No;

        return new Settings(isBlockedByAdmin, isDeleted);
    }


    internal ErrorOr<Settings> BlockByAdmin()
        => ChangeProperty(
            IsBlockedByAdmin,
            IsBlockedByAdmin.Yes,
            Errors.ValueObjects.Settings.AlreadyBlockedByAdmin,
            isBlocked => this with { IsBlockedByAdmin = IsBlockedByAdmin.Yes });

    internal ErrorOr<Settings> UnblockByAdmin()
        => ChangeProperty(
            IsBlockedByAdmin,
            IsBlockedByAdmin.No,
            Errors.ValueObjects.Settings.NotBlockedByAdmin,
            isBlocked => this with { IsBlockedByAdmin = IsBlockedByAdmin.No });

    internal ErrorOr<Settings> DeleteFriendshipList()
        => ChangeProperty(
            IsDeleted,
            IsDeleted.Yes,
            Errors.ValueObjects.Settings.UserAlreadyDeleted,
            isDeleted => this with { IsDeleted = IsDeleted.Yes });

    internal ErrorOr<Settings> RestoreFriendshipList()
        => ChangeProperty(
            IsDeleted,
            IsDeleted.No,
            Errors.ValueObjects.Settings.UserNotDeleted,
            isDeleted => this with { IsDeleted = IsDeleted.No });

    private static ErrorOr<Settings> ChangeProperty<T>(
            T currentValue,
            T newValue,
            Func<T, Error> errorFunc,
            Func<T, Settings> changeFunc)
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            return errorFunc(newValue);

        return changeFunc(newValue);
    }

    private Settings()
    {
    }
}
