using System.Text.Json;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public static class FriendshipCollectionJsonFactorys
{
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        Converters =
        {
            new FriendIdJsonConverter(),
            new InvitationIdJsonConverter(),
            new UserIdJsonConverter(),
            new FriendshipStateJsonConverter(),
            new FriendshipLevelJsonConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
