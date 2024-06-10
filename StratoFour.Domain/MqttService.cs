using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Internal;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StratoFour.Domain;
public class MqttService : BackgroundService
{
    private IMqttClient _mqttClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //MQTT Static variables
        string serverString = "localhost";
        int defaultPort = 1883;

        //maybe needed
        //string broker = "******.emqxsl.com";
        //string clientId = Guid.NewGuid().ToString();
        //string topic = "Csharp/mqtt";
        //string username = "emqxtest";
        //string password = "******";

        //Create MQTT Client Factory
        var factory = new MqttFactory();

        //Create MQTT client Instance
        _mqttClient = factory.CreateMqttClient();

        //Create Client Options
        var options = new MqttClientOptionsBuilder()
           .WithTcpServer(serverString, defaultPort) // MQTT broker address and port
                                                     //.WithCredentials(username, password) // Set username and password
                                                     //.WithClientId(clientId)
           .WithCleanSession()
           .Build();
    }
        

    public async Task SendRequestAsync(int player)
    {
        string playerCommand = "4$" + player.ToString();
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("send_from_app/position")
            .WithPayload(playerCommand)
            .WithRetainFlag()
            .Build();

        if (_mqttClient.IsConnected)
        {
            await _mqttClient.PublishAsync(message);
        }
    }
}
