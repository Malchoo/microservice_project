using Ardalis.SmartEnum;

namespace Friendships.Write.Domain.Enums;

public sealed class FriendshipLevel : SmartEnum<FriendshipLevel>
{
    public static readonly FriendshipLevel Standard = new(nameof(Standard), 0);
    public static readonly FriendshipLevel Trusted = new(nameof(Trusted), 1);

    private FriendshipLevel(string name, int value)
        : base(name, value)
    {
    }
}
