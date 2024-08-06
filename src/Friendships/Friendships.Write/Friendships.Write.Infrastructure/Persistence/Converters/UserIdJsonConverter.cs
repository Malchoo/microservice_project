using Friendships.Write.Domain.Ids;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class UserIdJsonConverter : JsonConverter<UserId>
{
    public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guidValue = reader.GetString();
        return new UserId(Guid.Parse(guidValue));
    }

    public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}