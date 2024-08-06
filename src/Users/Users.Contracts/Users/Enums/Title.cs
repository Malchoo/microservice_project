using System.Text.Json.Serialization;

namespace Users.Contracts.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum Title
{
    None,
    Mr,
    Mrs,
    Ms,
    Mx
}