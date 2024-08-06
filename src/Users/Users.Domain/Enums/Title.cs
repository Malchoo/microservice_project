using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class Title(string name, int value) : SmartEnum<Title>(name, value)
{
    public static readonly Title None = new Title(nameof(None), 0);
    public static readonly Title Mr = new Title(nameof(Mr), 1);
    public static readonly Title Mrs = new Title(nameof(Mrs), 2);
    public static readonly Title Ms = new Title(nameof(Ms), 3);
    public static readonly Title Mx = new Title(nameof(Mx), 4);
}