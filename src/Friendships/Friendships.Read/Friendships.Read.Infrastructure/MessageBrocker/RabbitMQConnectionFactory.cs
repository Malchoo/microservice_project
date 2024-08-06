using Friendships.Read.Infrastructure.Contracts;
using Friendships.Read.Infrastructure.IntegrationEvents.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Friendships.Read.Infrastructure.MessageBrocker;

public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
    private readonly MessageBrokerSettings _settings;
    private readonly ILogger<RabbitMQConnectionFactory> _logger;

    public RabbitMQConnectionFactory(
        IOptions<MessageBrokerSettings> options, 
        ILogger<RabbitMQConnectionFactory> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public IConnection CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ connection");
            throw;
        }
    }
}