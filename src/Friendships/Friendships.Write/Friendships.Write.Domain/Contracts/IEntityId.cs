namespace Friendships.Write.Domain.Contracts;

public interface IEntityId
{
    public Guid Value { get; init; }
}
