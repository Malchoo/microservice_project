using Ardalis.SmartEnum;

namespace Friendships.Write.Domain.Enums;

public sealed class FriendshipState : SmartEnum<FriendshipState>
{
    public static readonly FriendshipState Active = new(nameof(Active), 0);
    public static readonly FriendshipState Ended = new(nameof(Ended), 1);

    private FriendshipState(string name, int value)
        : base(name, value)
    {
    }
}
