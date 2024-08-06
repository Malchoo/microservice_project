using Users.Domain.Contracts;

namespace Users.Domain.Ids;

public readonly record struct AdminId(Guid Value) : IEntityId
{
    public static AdminId CreateUnique() => new(Guid.NewGuid());

    public static AdminId Empty() => new(Guid.Empty);
}