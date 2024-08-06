using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class UniqueIdCollectionValueComparer<TId> : ValueComparer<UniqueIdCollection<TId>>
    where TId : IEntityId
{
    public UniqueIdCollectionValueComparer()
        : base(
            (c1, c2) => c1.Ids.SequenceEqual(c2.Ids),
            c => c.Ids.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => CreateSnapshot(c))
    {
    }

    private static UniqueIdCollection<TId> CreateSnapshot(UniqueIdCollection<TId> source)
    {
        var snapshot = UniqueIdCollection<TId>.Empty<TId>();
        foreach (var id in source.Ids)
        {
            var result = snapshot.Add(id);
            if (result.IsError)
            {
                throw new InvalidOperationException($"Failed to add id to snapshot: {result.FirstError.Description}");
            }
        }
        return snapshot;
    }
}