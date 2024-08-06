using Users.Domain.Contracts;

namespace Users.Domain.Ids;

public readonly record struct SuperAdminId(Guid Value) : IEntityId
{
    public static SuperAdminId CreateUnique() => new(Guid.NewGuid());

    public static SuperAdminId Empty() => new(Guid.Empty);
}