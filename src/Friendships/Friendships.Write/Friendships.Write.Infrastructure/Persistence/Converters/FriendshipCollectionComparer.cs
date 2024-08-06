using Friendships.Write.Domain.Entities;
using Friendships.Write.Domain.Ids;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class FriendshipCollectionComparer : ValueComparer<IReadOnlyDictionary<FriendId, Friendship>>
{
    public FriendshipCollectionComparer()
        : base(
            (d1, d2) => CompareDictionaries(d1, d2),
            d => GetDictionaryHashCode(d),
            d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    {
    }

    private static bool CompareDictionaries(IReadOnlyDictionary<FriendId, Friendship> d1, IReadOnlyDictionary<FriendId, Friendship> d2)
    {
        if (d1.Count != d2.Count)
            return false;

        foreach (var kvp in d1)
        {
            if (!d2.TryGetValue(kvp.Key, out var value))
                return false;

            if (!kvp.Value.Equals(value))
                return false;
        }

        return true;
    }

    private static int GetDictionaryHashCode(IReadOnlyDictionary<FriendId, Friendship> dictionary)
    {
        int hash = 0;
        foreach (var kvp in dictionary)
        {
            hash = HashCode.Combine(hash, kvp.Key, kvp.Value);
        }

        return hash;
    }
}