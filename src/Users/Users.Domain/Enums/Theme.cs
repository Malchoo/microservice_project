using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class Theme(string name, int value) : SmartEnum<Theme>(name, value)
{
    public static readonly Theme Light = new(nameof(Light), 0);
    public static readonly Theme Dark = new(nameof(Dark), 1);
}
