using System.Net.Http.Json;

namespace NotificationService
{
    public record HealthResponseDTO(string apiVersion, string status, DateTime currentTime);

    public class Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = httpClientFactory.CreateClient();

            logger.LogInformation("Waiting for web api to start...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var response = await client.GetAsync("https://localhost:7094/api/utils/health", stoppingToken);
                    response.EnsureSuccessStatusCode();

                    var health = await response.Content.ReadFromJsonAsync<HealthResponseDTO>(cancellationToken: stoppingToken);

                    logger.LogInformation("Health check: {ApiVersion} {Status} {CurrentTime}",
                        health?.apiVersion, health?.status, health?.currentTime);
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Health check: {ex.Message}");
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
