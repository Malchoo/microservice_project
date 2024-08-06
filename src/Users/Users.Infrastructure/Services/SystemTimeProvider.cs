using Users.Domain.Time;

namespace Users.Infrastructure.Services;

public sealed class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime UtcNow() => DateTime.UtcNow;
}
