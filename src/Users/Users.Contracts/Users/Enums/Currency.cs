using System.Text.Json.Serialization;

namespace Users.Contracts.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum Currency
{
    BGN,
    EU,
    USD
}