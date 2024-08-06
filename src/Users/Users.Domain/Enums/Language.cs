using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class Language(string name, int value) : SmartEnum<Language>(name, value)
{
    public static readonly Language BG = new(nameof(BG), 0);
    public static readonly Language EN = new(nameof(EN), 1);
    public static readonly Language DE = new(nameof(DE), 2);
}
