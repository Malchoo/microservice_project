using Friendships.Write.Domain.Contracts;

namespace Friendships.Write.Domain.Primitives;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; } //ToDo PROTECTED SETTER
    //public DateTime? CreatedAt { get; set; }
    //public string? CreatedBy { get; set; }
    //public DateTime? LastModified { get; set; }
    //public string? LastModifiedBy { get; set; }
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> other)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
