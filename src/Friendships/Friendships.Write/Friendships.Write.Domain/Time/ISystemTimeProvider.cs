namespace Friendships.Write.Domain.Time;

public interface ISystemTimeProvider
{
    DateTime UtcNow();
}