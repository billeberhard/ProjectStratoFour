//using System.Text;
//using Microsoft.Extensions.Logging;
//using MQTTnet;
//using MQTTnet.Client;

//namespace StratoFour.Domain;

//public class MqttService
//{
//    private readonly ILogger<MqttService> _logger;
//    private IMqttClient _mqttClient;
//    private MqttClientOptions _options;

//    public MqttService(ILogger<MqttService> logger)
//    {
//        _logger = logger;

//        var factory = new MqttFactory();
//        _mqttClient = factory.CreateMqttClient();
//        _options = new MqttClientOptionsBuilder()
//            .WithTcpServer("localhost", 1883)
//            .Build();

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
//            await Task.Delay(TimeSpan.FromSeconds(5)); // Wartezeit vor erneutem Verbindungsversuch
//            try
//            {
//                await _mqttClient.ConnectAsync(_options);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error while reconnecting to MQTT Broker.");
//            }
//        };

//        _mqttClient.ApplicationMessageReceivedAsync += e =>
//        {
//            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
//            _logger.LogInformation($"Received message: {message} from topic: {e.ApplicationMessage.Topic}");

//            // Process received message
//            return Task.CompletedTask;
//        };
//    }

//    public async Task ConnectAsync(CancellationToken stoppingToken)
//    {
//        try
//        {
//            if (!_mqttClient.IsConnected)
//            {
//                await _mqttClient.ConnectAsync(_options, stoppingToken);
//            }
//        }
//        catch (MQTTnet.Exceptions.MqttCommunicationException ex)
//        {
//            _logger.LogError(ex, "Error while connecting to MQTT Broker.");
//            throw;
//        }
//    }

//    public async Task DisconnectAsync()
//    {
//        await _mqttClient.DisconnectAsync();
//    }

//    public async Task SendPlayerTurnRequestAsync(int player, int row)
//    {
//        string playerCommand = $"{row}${player}";
//        var message = new MqttApplicationMessageBuilder()
//            .WithTopic("send_from_app/position")
//            .WithPayload(playerCommand)
//            .WithRetainFlag()
//            .Build();

//        if (_mqttClient.IsConnected)
//        {
//            await _mqttClient.PublishAsync(message);
//        }
//        else
//        {
//            _logger.LogWarning("Cannot send message, MQTT client is not connected.");
//        }
//    }
//}