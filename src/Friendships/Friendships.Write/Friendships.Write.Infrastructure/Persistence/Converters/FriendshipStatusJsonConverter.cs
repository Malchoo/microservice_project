using Friendships.Write.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class FriendshipStateJsonConverter : JsonConverter<FriendshipState>
{
    public override FriendshipState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return FriendshipState.FromName(value);
    }

    public override void Write(Utf8JsonWriter writer, FriendshipState value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}
