using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class UniqueIdCollectionComparer<TId> : ValueComparer<UniqueIdCollection<TId>>
    where TId : IEntityId
{
    public UniqueIdCollectionComparer()
        : base(
            (c1, c2) => c1.Ids.SequenceEqual(c2.Ids),
            c => c.Ids.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => CreateCopy(c))
    {
    }

    private static UniqueIdCollection<TId> CreateCopy(UniqueIdCollection<TId> original)
    {
        var copy = UniqueIdCollection<TId>.Empty<TId>();
        foreach (var id in original.Ids)
        {
            var result = copy.Add(id);

            if (result.IsError)
            {
                throw new InvalidOperationException($"Failed to add id to snapshot: {result.FirstError.Description}");
            }
        }
        return copy;
    }
}