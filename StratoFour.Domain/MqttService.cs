using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace name.Domain
{
    public class MqttService
    {
        private IMqttClient _mqttClient;

        public async Task StartAsync()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("your_mqtt_broker_address") // MQTT Broker Adresse hier einfügen
                .Build();

            //_mqttClient.UseConnectedHandler(async e =>
            //{
            //    Console.WriteLine("Connected to MQTT broker.");
            //    // Hier könntest du weitere Aktionen ausführen, sobald die Verbindung hergestellt ist
            //});

            //_mqttClient.UseDisconnectedHandler(async e =>
            //{
            //    Console.WriteLine("Disconnected from MQTT broker.");
            //    // Hier könntest du Logik für den Umgang mit einer getrennten Verbindung implementieren
            //});

            //_mqttClient.UseApplicationMessageReceivedHandler(e =>
            //{
            //    Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            //    // Hier könntest du Logik zum Verarbeiten von empfangenen Nachrichten implementieren
            //});

            await _mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        public async Task PublishAsync(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                //.WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
        }

        public async Task StopAsync()
        {
            await _mqttClient.DisconnectAsync();
        }
    }
}
