using ErrorOr;
using Users.Domain.Errors.Settings;
using Users.Domain.ValueObjects.ValueType;

namespace Users.Domain.ValueObjects.ReferenceType;

public sealed record Settings
{
    private Settings(
        IsVerified isVerified,
        IsBlockedByAdmin isBlockedByAdmin,
        IsDeleted isDeleted)
    {
        IsVerified = isVerified;
        IsBlockedByAdmin = isBlockedByAdmin;
        IsDeleted = isDeleted;
    }

    public IsVerified IsVerified { get; init; }
    public IsBlockedByAdmin IsBlockedByAdmin { get; init; }
    public IsDeleted IsDeleted { get; init; }

    internal static ErrorOr<Settings> Create()
    {
        var isVerified = IsVerified.Yes;    //ToDo this should be NO

        var isBlockedByAdmin = IsBlockedByAdmin.No;

        var isDeleted = IsDeleted.No;

        return new Settings(isVerified, isBlockedByAdmin, isDeleted);
    }

    internal ErrorOr<Settings> ChangeIsVerified()
        => ChangeProperty(
            IsVerified,
            IsVerified.Yes,
            SettingsErrors.IsVerifiedNotChanged,
            isVerified => this with { IsVerified = IsVerified.Yes });

    internal ErrorOr<Settings> Unverified()
        => ChangeProperty(
            IsVerified,
            IsVerified.No,
            SettingsErrors.UnverifiedAnyway,
             isVerified => this with { IsVerified = IsVerified.No });

    internal ErrorOr<Settings> BlockByAdmin()
        => ChangeProperty(
            IsBlockedByAdmin,
            IsBlockedByAdmin.Yes,
            SettingsErrors.AlreadyBlockedByAdmin,
            isBlocked => this with { IsBlockedByAdmin = IsBlockedByAdmin.Yes });

    internal ErrorOr<Settings> UnblockByAdmin()
        => ChangeProperty(
            IsBlockedByAdmin,
            IsBlockedByAdmin.No,
            SettingsErrors.NotBlockedByAdmin,
            isBlocked => this with { IsBlockedByAdmin = IsBlockedByAdmin.No });

    internal ErrorOr<Settings> Delete()
        => ChangeProperty(
            IsDeleted,
            IsDeleted.Yes,
            SettingsErrors.UserAlreadyDeleted,
            isDeleted => this with { IsDeleted = IsDeleted.Yes });

    internal ErrorOr<Settings> Restore()
        => ChangeProperty(
            IsDeleted,
            IsDeleted.No,
            SettingsErrors.UserNotDeleted,
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
