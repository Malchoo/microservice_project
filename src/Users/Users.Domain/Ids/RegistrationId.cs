using Users.Domain.Contracts;

namespace Users.Domain.Ids;

public readonly record struct RegistrationId(Guid Value) : IEntityId
{
    public static RegistrationId CreateUnique() => new(Guid.NewGuid());
}