using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace StratoFour.Domain
{
    public class BackGroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackGroundWorkerService> _logger;
        private readonly IMqttClient _mqttClient;
        private readonly MessageService _messageService; 

        public BackGroundWorkerService(ILogger<BackGroundWorkerService> logger, MessageService messageService)
        {
            _logger = logger;
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _messageService = messageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) // MQTT Broker Adresse und Port
                .Build();

            // Event-Handler für empfangene MQTT-Nachrichten setzen
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                if (e.ApplicationMessage.Topic == "send_from_arduino/positioned")
                {
                    string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    _logger.LogInformation($"Received message on topic '{e.ApplicationMessage.Topic}': {message}");
                    _messageService.Publish(message);
                }
            });

            try
            {
                await _mqttClient.ConnectAsync(options, stoppingToken);
                _logger.LogInformation("Connected to MQTT broker.");

                // Thema abonnieren
                await _mqttClient.SubscribeAsync("send_from_arduino/positioned");

                while (!stoppingToken.IsCancellationRequested)
                {
                    // Hier kann Ihre spezifische Logik implementiert werden
                    _logger.LogInformation("MqttBackgroundService running");
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while connecting to MQTT broker.");
            }
            finally
            {
                await _mqttClient.DisconnectAsync();
                _logger.LogInformation("Disconnected from MQTT broker.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.DisconnectAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
