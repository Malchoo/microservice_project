using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class Role(string name, int value) : SmartEnum<Role>(name, value)
{
    public static readonly Role User = new(nameof(User), 0);
    public static readonly Role Admin = new(nameof(Admin), 1);
    public static readonly Role SuperAdmin = new(nameof(SuperAdmin), 2);
}
