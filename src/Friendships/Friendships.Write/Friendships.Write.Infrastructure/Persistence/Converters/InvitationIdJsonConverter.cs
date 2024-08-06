using Friendships.Write.Domain.Ids;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class InvitationIdJsonConverter : JsonConverter<InvitationId>
{
    public override InvitationId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guidValue = reader.GetString();
        return new InvitationId(Guid.Parse(guidValue));
    }

    public override void Write(Utf8JsonWriter writer, InvitationId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}
