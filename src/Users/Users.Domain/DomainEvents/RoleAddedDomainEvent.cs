using Users.Domain.Contracts;
using Users.Domain.Entities;
using Users.Domain.Enums;

namespace Users.Domain.DomainEvents;

public sealed record RoleAddedDomainEvent(
    User User, Enums.Role Role) : IDomainEvent;
