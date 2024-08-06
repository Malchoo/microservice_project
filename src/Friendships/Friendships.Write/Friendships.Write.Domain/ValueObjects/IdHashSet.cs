using ErrorOr;
using Friendships.Domain.DomainErrors;
using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Primitives;

namespace Friendships.Write.Domain.ValueObjects;

public sealed class UniqueIdCollection<TId> : ValueObject
    where TId : IEntityId
{
    private readonly HashSet<TId> _ids = new();
    private Type? _idType;

    private UniqueIdCollection(Type idType) => _idType = idType;

    public static ErrorOr<UniqueIdCollection<TId>> Create(IEnumerable<Guid> idList)
    {
        //ToDo: оправи имената
        var result = Empty<TId>();
        foreach (var guid in idList)
        {
            var id = (TId)Activator.CreateInstance(typeof(TId), guid);
            var addIdResult = result.Add(id);
            if (addIdResult.IsError)
                return addIdResult.Errors;
        }
        return result;
    }

    public static UniqueIdCollection<TId> Empty<T>() where T : TId =>
        new UniqueIdCollection<TId>(typeof(T));

    public int Count => _ids.Count;

    public IReadOnlyCollection<TId> Ids => _ids.ToList().AsReadOnly();

    public ErrorOr<Success> Add(TId id)
    {
        if (IsEmptyGuid(id))
            return Errors.ValueObjects.UniqueIdCollection.NotEmptyGuid;

        if (_idType == null)
        {
            _idType = id.GetType();
        }

        if (id.GetType() != _idType)
            return Errors.ValueObjects.UniqueIdCollection.TypeMismatch(_idType.Name, id.GetType().Name);

        if (Contains(id))
            return Errors.ValueObjects.UniqueIdCollection.AlreadyInUniqueIdCollection;

        _ids.Add(id);

        return Result.Success;
    }

    internal ErrorOr<Success> Remove(TId id)
    {
        if (!Contains(id))
            return Errors.ValueObjects.UniqueIdCollection.NotFound;

        _ids.Remove(id);

        return new Success();
    }

    internal bool Contains(TId id) => _ids.Contains(id);

    private static bool IsEmptyGuid(TId id) => id.Value == Guid.Empty;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        return _ids.OrderBy(id => id.Value).Cast<object?>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not UniqueIdCollection<TId> other)
            return false;

        return _ids.SetEquals(other._ids);
    }

    public override int GetHashCode()
    {
        return _ids.Aggregate(0, (hash, id) => hash ^ id.GetHashCode());
    }
}