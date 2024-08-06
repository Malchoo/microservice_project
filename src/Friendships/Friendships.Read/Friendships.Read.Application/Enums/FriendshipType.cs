using Ardalis.SmartEnum;

namespace Friendships.Read.Application.Enums;

public sealed class FriendshipType : SmartEnum<FriendshipType>
{
    public static readonly FriendshipType Standard = new(nameof(Standard), 0);
    public static readonly FriendshipType Trusted = new(nameof(Trusted), 1);

    private FriendshipType(string name, int value)
        : base(name, value)
    {
    }
}
