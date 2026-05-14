using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService
{
    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        private const string QueueName = "logs";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
                UserName = configuration.GetValue<string>("RabbitMq:Username") ?? "admin",
                Password = configuration.GetValue<string>("RabbitMq:Password") ?? "admin"
            };

            using var connection = await factory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

            var sessionFilePath = Path.Combine(AppContext.BaseDirectory, $"logs-{DateTime.UtcNow:yyyyMMdd-HHmmss}.txt");
            await using var fileStream = new FileStream(sessionFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(fileStream) { AutoFlush = true };

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.Span);
                await writer.WriteLineAsync($"{DateTime.Now} - {message}");
            };

            await channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

            logger.LogInformation("Logging worker started. Writing logs to {Path}", sessionFilePath);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
