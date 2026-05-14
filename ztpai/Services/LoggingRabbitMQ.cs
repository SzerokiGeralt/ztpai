using RabbitMQ.Client;
using System.Text;

namespace ztpai.Services
{
    public class LoggingRabbitMQ(IConfiguration configuration) : ILoggingService
    {
        private const string QueueName = "logs";

        public void Log(string payload)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
                UserName = configuration.GetValue<string>("RabbitMq:Username") ?? "admin",
                Password = configuration.GetValue<string>("RabbitMq:Password") ?? "admin"
            };

            using var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            using var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

            channel.QueueDeclareAsync(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null)
                .GetAwaiter().GetResult();

            var body = Encoding.UTF8.GetBytes(payload);
            channel.BasicPublishAsync(exchange: string.Empty, routingKey: QueueName, mandatory: false, basicProperties: new BasicProperties(), body: body)
                .GetAwaiter().GetResult();
        }
    }
}
