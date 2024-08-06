namespace Users.Domain.Time;

public interface ISystemTimeProvider
{
    DateTime UtcNow();
}