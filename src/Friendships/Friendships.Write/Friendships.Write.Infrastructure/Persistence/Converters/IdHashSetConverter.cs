using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Friendships.Write.Infrastructure.Persistence.Converters;

public class UniqueIdCollectionConverter<TId> : ValueConverter<UniqueIdCollection<TId>, string>
    where TId : IEntityId
{
    public UniqueIdCollectionConverter()
        : base(
            v => SerializeUniqueIdCollection(v),
            v => DeserializeUniqueIdCollection<TId>(v))
    {
    }

    private static string SerializeUniqueIdCollection(UniqueIdCollection<TId> uniqueIdCollection)
    {
        var ids = uniqueIdCollection.Ids.Select(id => id.Value).ToList();
        return JsonSerializer.Serialize(ids);
    }

    private static UniqueIdCollection<TId> DeserializeUniqueIdCollection<TId>(string json) where TId : IEntityId
    {
        var ids = JsonSerializer.Deserialize<List<Guid>>(json);

        var uniqueIdCollection = UniqueIdCollection<TId>.Empty<TId>();

        foreach (var id in ids)
        {
            var entityId = (TId)Activator.CreateInstance(typeof(TId), id);

            if(entityId == null)            
                throw new JsonException($"Failed to create entity id from guid: {id}");
            

            var result = uniqueIdCollection.Add(entityId);

            if(result.IsError)            
                throw new JsonException($"Failed to add id to snapshot: {result.FirstError.Description}");
            
        }
        return uniqueIdCollection;
    }
}