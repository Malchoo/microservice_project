using Friendships.Write.Domain.Ids;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class FriendIdJsonConverter : JsonConverter<FriendId>
{
    public override FriendId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new FriendId(reader.GetGuid());
    }

    public override void Write(Utf8JsonWriter writer, FriendId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}