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
    private IMqttClient _mqttClient;
    private MqttClientOptions _options;

    public BackGroundWorkerService(ILogger<BackGroundWorkerService> logger)
    {
        _logger = logger;

        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();
        _options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    try
        //    {
        //        _mqttClient.ConnectedAsync += async e =>
        //        {
        //            _logger.LogInformation("Connected successfully with MQTT Broker(s).");

        //            // Subscribe to a topic
        //            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("send_from_app/position_response").Build());

        //            _logger.LogInformation("Subscribed to topic.");
        //        };

        //        _mqttClient.DisconnectedAsync += async e =>
        //        {
        //            _logger.LogInformation("Disconnected from MQTT Broker(s). Reconnecting...");
        //            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wartezeit vor erneutem Verbindungsversuch
        //        };

        //        _mqttClient.ApplicationMessageReceivedAsync += e =>
        //        {
        //            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        //            _logger.LogInformation($"Received message: {message} from topic: {e.ApplicationMessage.Topic}");

        //            // Process received message
        //            return Task.CompletedTask;
        //        };

        //        await _mqttClient.ConnectAsync(_options, stoppingToken);

        //        while (_mqttClient.IsConnected && !stoppingToken.IsCancellationRequested)
        //        {
        //            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //            await Task.Delay(1000, stoppingToken);
        //        }
        //    }
        //    catch (MQTTnet.Exceptions.MqttCommunicationException ex)
        //    {
        //        _logger.LogError(ex, "Error while connecting to MQTT Broker. Retrying in 5 seconds...");
        //        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wartezeit vor erneutem Verbindungsversuch
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected error occurred. Retrying in 5 seconds...");
        //        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wartezeit vor erneutem Verbindungsversuch
        //    }
        //}

        //await _mqttClient.DisconnectAsync();
    }

    public async Task SendPlayerTurnRequestAsync(int player, int row)
    {
        //    string playerCommand = row.ToString() + "$" + player.ToString();
        //    var message = new MqttApplicationMessageBuilder()
        //        .WithTopic("send_from_app/position")
        //        .WithPayload(playerCommand)
        //        .WithRetainFlag()
        //        .Build();

        //    if (_mqttClient.IsConnected)
        //    {
        //        await _mqttClient.PublishAsync(message);
        //    }
        //    else
        //    {
        //        _logger.LogWarning("Cannot send message, MQTT client is not connected.");
        //    }
    }
}
