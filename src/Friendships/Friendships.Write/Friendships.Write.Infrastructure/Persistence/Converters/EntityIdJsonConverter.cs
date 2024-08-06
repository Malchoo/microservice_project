using Friendships.Write.Domain.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Friendships.Write.Infrastructure.Persistence.Converters;
public class EntityIdJsonConverter : JsonConverter<IEntityId>
{
    public override IEntityId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        Guid value = Guid.Empty;
        string typeName = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (value == Guid.Empty || string.IsNullOrEmpty(typeName))
                {
                    throw new JsonException();
                }

                Type type = Type.GetType(typeName);
                return (IEntityId)Activator.CreateInstance(type, value);
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Value":
                        value = reader.GetGuid();
                        break;
                    case "Type":
                        typeName = reader.GetString();
                        break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IEntityId value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Type", value.GetType().AssemblyQualifiedName);
        writer.WriteString("Value", value.Value);
        writer.WriteEndObject();
    }
}