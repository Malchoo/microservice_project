using Friendships.Read.Application.Contracts;
using Friendships.Read.Infrastructure.Contracts;
using Friendships.Read.Infrastructure.Dapper;
using Friendships.Read.Infrastructure.IntegrationEvents.BackgroundService;
using Friendships.Read.Infrastructure.IntegrationEvents.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Data;

namespace Friendships.Read.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddOptions();


        services.AddSingleton<IDbConnection>(sp =>
        {
            var connectionString = configuration.GetConnectionString("FriendshipsReadDatabase");
            return new SqlConnection(connectionString);
        });

        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var connectionString = configuration.GetConnectionString("FriendshipsReadDatabase");
            return new ConnectionFactory(connectionString!);
        });

        // RabbitMQ configuration
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.Section));

        // Background services
        services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });

        return services;
    }

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var messageBrokerSettings = new MessageBrokerSettings();
        configuration.Bind(MessageBrokerSettings.Section, messageBrokerSettings);

        services.AddSingleton(Options.Create(messageBrokerSettings));

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        return services.AddHostedService<ConsumeIntegrationEventsBackgroundService>();
    }
}