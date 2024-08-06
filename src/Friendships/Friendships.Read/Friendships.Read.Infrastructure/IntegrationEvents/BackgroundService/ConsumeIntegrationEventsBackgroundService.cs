using Friendships.Read.Infrastructure.IntegrationEvents.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Friendships.Read.Infrastructure.IntegrationEvents.BackgroundService;

public class ConsumeIntegrationEventsBackgroundService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ConsumeIntegrationEventsBackgroundService> _logger;
    private readonly MessageBrokerSettings _messageBrokerSettings;
    private IConnection _connection;
    private IModel _channel;
    private bool _disposed;

    public ConsumeIntegrationEventsBackgroundService(
        ILogger<ConsumeIntegrationEventsBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<MessageBrokerSettings> messageBrokerOptions)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _messageBrokerSettings = messageBrokerOptions.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting integration event consumer background service.");

        try
        {
            ConnectToRabbitMQ();
            ConfigureRabbitMQConsumer();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start RabbitMQ consumer. Service will attempt to reconnect.");
            Task.Run(async () => await TryReconnect(cancellationToken));
        }

        return Task.CompletedTask;
    }

    private void ConnectToRabbitMQ()
    {
        _logger.LogInformation("Connecting to RabbitMQ. HostName: {HostName}, Port: {Port}", _messageBrokerSettings.HostName, _messageBrokerSettings.Port);

        var factory = new ConnectionFactory
        {
            HostName = _messageBrokerSettings.HostName,
            Port = _messageBrokerSettings.Port,
            UserName = _messageBrokerSettings.UserName,
            Password = _messageBrokerSettings.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _logger.LogInformation("Successfully connected to RabbitMQ.");
    }

    private void ConfigureRabbitMQConsumer()
    {
        _channel.QueueDeclare(
            queue: _messageBrokerSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            _messageBrokerSettings.QueueName,
            _messageBrokerSettings.ExchangeName,
            routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnEventReceived;

        _channel.BasicConsume(_messageBrokerSettings.QueueName, autoAck: false, consumer);

        _logger.LogInformation("RabbitMQ consumer configured and started.");
    }

    private async Task OnEventReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            _logger.LogInformation("Received integration event. Processing message from queue.");

            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var scope = _serviceScopeFactory.CreateScope();
            // Here you would typically deserialize the message and process it
            // For example:
            // var eventProcessor = scope.ServiceProvider.GetRequiredService<IIntegrationEventProcessor>();
            // await eventProcessor.ProcessEvent(message);

            _logger.LogInformation("Integration event processed successfully.");

            _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing integration event");
            _channel.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);
        }
    }

    private async Task TryReconnect(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                ConnectToRabbitMQ();
                ConfigureRabbitMQConsumer();
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reconnect to RabbitMQ. Retrying in 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping integration event consumer background service.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        _disposed = true;
    }
}