using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class UniqueIdCollectionValueConverter<TId> : ValueConverter<UniqueIdCollection<TId>, string>
    where TId : IEntityId
{
    public UniqueIdCollectionValueConverter()
        : base(
            v => SerializeUniqueIdCollection(v),
            v => DeserializeUniqueIdCollection<TId>(v))
    {
    }

    private static string SerializeUniqueIdCollection(UniqueIdCollection<TId> uniqueIdCollection)
    {
        var guids = uniqueIdCollection.Ids.Select(id => id.Value).ToList();
        return JsonSerializer.Serialize(guids);
    }

    private static UniqueIdCollection<TId> DeserializeUniqueIdCollection<TId>(string json) where TId : IEntityId
    {
        var guids = JsonSerializer.Deserialize<List<Guid>>(json);
        var uniqueIdCollection = UniqueIdCollection<TId>.Empty<TId>();
        foreach (var guid in guids)
        {
            var id = (TId)Activator.CreateInstance(typeof(TId), guid);
            var result = uniqueIdCollection.Add(id);
            if (result.IsError)
            {
                // Тук можете да решите как да се справите с грешката. 
                // Например, можете да хвърлите изключение или да логнете грешката.
                throw new InvalidOperationException($"Failed to add id: {result.FirstError.Description}");
            }
        }
        return uniqueIdCollection;
    }
}