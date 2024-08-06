using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Time;
using Friendships.Write.Infrastructure;
using Friendships.Write.Infrastructure.IntegrationEvents.BackgroundService;
using Friendships.Write.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;
using Friendships.Write.Infrastructure.IntegrationEvents.Settings;
using Friendships.Write.Infrastructure.Persistence;
using Friendships.Write.Infrastructure.Persistence.Repositories;
using Friendships.Write.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Friendships.Write.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
            throw new InvalidOperationException("Connection string 'FriendshipWriteDatabase' not found.");

        services
            .AddMediatR()
            .AddConfigurations(configuration)
            .AddBackgroundServices()
            .AddPersistence(connectionString!);

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var messageBrokerSettings = new MessageBrokerSettings();
        configuration.Bind(MessageBrokerSettings.Section, messageBrokerSettings);

        services.AddSingleton(Options.Create(messageBrokerSettings));

        return services;
    }

    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<IIntegrationEventsPublisher, IntegrationEventsPublisher>();
        services.AddHostedService<PublishIntegrationEventsBackgroundService>();
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FriendshipListDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IFriendshipListRepository, FriendshipListRepository>();
        services.AddTransient<ISystemTimeProvider, SystemTimeProvider>();

        return services;
    }
}