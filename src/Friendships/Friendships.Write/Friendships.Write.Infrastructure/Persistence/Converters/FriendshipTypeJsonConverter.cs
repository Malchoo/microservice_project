using Friendships.Write.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class FriendshipLevelJsonConverter : JsonConverter<FriendshipLevel>
{
    public override FriendshipLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return FriendshipLevel.FromName(value);
    }

    public override void Write(Utf8JsonWriter writer, FriendshipLevel value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}
