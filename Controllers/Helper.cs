using MQTTnet;
using MQTTnet.Client;

namespace somiod.Controllers{
    public class Helper{
        public static async Task PublishAsync(string broker, string topic, string message){
            var mqttFactory = new MqttFactory();
            using( var mqttClient = mqttFactory.CreateMqttClient() ){
                var options = new MqttClientOptionsBuilder().WithTcpServer(broker).Build();
                try {
                    using (var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(1))){
                        await mqttClient.ConnectAsync(options, timeoutToken.Token);
                    }
                }
                catch (Exception e){
                    Console.WriteLine(e);
                }
                var payload = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(message).Build();
                await mqttClient.PublishAsync(payload);
                await mqttClient.DisconnectAsync();
            }
        }
    }
}