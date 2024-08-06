using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class TwoFactorAuth(string name, int value) : SmartEnum<TwoFactorAuth>(name, value)
{
    public static readonly TwoFactorAuth SMS = new(nameof(SMS), 0);
    public static readonly TwoFactorAuth Email = new(nameof(Email), 1);
}
