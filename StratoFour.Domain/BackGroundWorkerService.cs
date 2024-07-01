using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Internal;

namespace StratoFour.Domain;
public class BackGroundWorkerService : BackgroundService
{
    private readonly ILogger<BackGroundWorkerService> _logger;
    private readonly MqttService _mqttService;

    public BackGroundWorkerService(ILogger<BackGroundWorkerService> logger, MqttService mqttService)
    {
        _logger = logger;

        var factory = new MqttFactory();
        _mqttService = mqttService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _mqttService.ConnectAsync(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred. Retrying in 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wartezeit vor erneutem Verbindungsversuch
            }
        }

        await _mqttService.DisconnectAsync();
    }
    public async Task SendPlayerTurnAsync(int player, int row)
    {
        var _player = player;
        var _row = row;
        _mqttService.SendPlayerTurnRequestAsync(_player, _row);
    }
}