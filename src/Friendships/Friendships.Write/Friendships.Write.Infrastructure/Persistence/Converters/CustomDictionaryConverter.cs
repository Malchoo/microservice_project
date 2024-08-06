using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class CustomDictionaryConverter : ValueConverter<Dictionary<FriendId, Friendship>, string>
{
    public CustomDictionaryConverter() : base(
        v => SerializeDictionary(v),
        v => DeserializeDictionary(v))
    {
    }

    private static string SerializeDictionary(Dictionary<FriendId, Friendship> dict)
    {
        var keyValuePairs = dict.ToDictionary(kvp => kvp.Key.Value, kvp => kvp.Value);
        return JsonSerializer.Serialize(keyValuePairs, FriendshipCollectionJsonFactorys.Options);
    }

    private static Dictionary<FriendId, Friendship> DeserializeDictionary(string json)
    {
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<Guid, Friendship>>(json, FriendshipCollectionJsonFactorys.Options);
        return keyValuePairs.ToDictionary(kvp => new FriendId(kvp.Key), kvp => kvp.Value);
    }
}
