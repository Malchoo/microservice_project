using System.Text.Json.Serialization;

namespace Friendships.Write.Contracts.Friendships.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum IsDeleted
{
    No,
    Yes
}
