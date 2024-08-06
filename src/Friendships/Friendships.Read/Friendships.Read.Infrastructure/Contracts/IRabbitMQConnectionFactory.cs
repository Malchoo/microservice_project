using RabbitMQ.Client;

namespace Friendships.Read.Infrastructure.Contracts;

public interface IRabbitMQConnectionFactory
{
    IConnection CreateConnection();
}
