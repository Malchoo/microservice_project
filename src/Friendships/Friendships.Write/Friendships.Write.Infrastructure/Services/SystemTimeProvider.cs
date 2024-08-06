using Friendships.Write.Domain.Time;

namespace Friendships.Write.Infrastructure.Services;

public sealed class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime UtcNow() => DateTime.UtcNow;
}
