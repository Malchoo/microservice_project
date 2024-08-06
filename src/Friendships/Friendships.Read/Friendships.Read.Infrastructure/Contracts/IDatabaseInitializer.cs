namespace Friendships.Read.Infrastructure.Contracts;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}
