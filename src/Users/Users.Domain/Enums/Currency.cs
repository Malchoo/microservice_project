using Ardalis.SmartEnum;

namespace Users.Domain.Enums;

public sealed class Currency: SmartEnum<Currency>
{
    public static readonly Currency BGN = new(nameof(BGN), 0);
    public static readonly Currency EU = new(nameof(EU), 1);
    public static readonly Currency USD = new(nameof(USD), 2);

    public Currency(string name, int value) : base(name, value)
    {
    }
}
