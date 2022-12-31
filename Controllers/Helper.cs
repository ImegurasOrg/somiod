using MQTTnet;
using MQTTnet.Client;
using somiod.DAL;
using somiod.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.Xml;

namespace somiod.Controllers{
    public class Helper{

        public static async Task PublishAsync(string connUri, string topic, DataDTO message){
            var mqttFactory = new MqttFactory();
            using( var mqttClient = mqttFactory.CreateMqttClient()){
                try {
                var options = new MqttClientOptionsBuilder().WithConnectionUri(connUri).Build();
                    using (var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(1))){
                        await mqttClient.ConnectAsync(options, timeoutToken.Token);
                    }
                }
                catch (Exception e){
                    if (e is ArgumentException){
                        if (e.Message.Contains("Unexpected scheme in uri")){
                            Console.WriteLine("Invalid URI");
                            return;
                        }
                    } else {
                        Console.WriteLine("Connection to MQTT broker failed.");
                    }
                    Console.WriteLine(e);
                    return;
                }
                string mqttMessage = XmlDataDtoToString(message);
                Console.WriteLine("Message: " + mqttMessage);
                var payload = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(mqttMessage).Build();
                await mqttClient.PublishAsync(payload);
                await mqttClient.DisconnectAsync();
            }
        }

        public static List<OmeuTipo>? CheckSubscritions(string module, InheritanceMappingContext _context){
            //query get subscriptions where module = mod
            //subctract from count created - count deleted
            //if count > 0 then send message(topic = mod, message = data)
            try{
                Console.WriteLine(_context.Subscriptions);
                var subscriptions =  _context.Subscriptions
                .Include(s => s.parent)
                .DefaultIfEmpty()
                .Where(s => s.parent.name == module)
                .GroupBy(s => s.endpoint);
                //TODO: make created and deleted into macro
                var result = subscriptions
                    .Select(g => new OmeuTipo(g.Key,g.Count(s => s.@event == "created") - g.Count(s => s.@event == "deleted")))
                    .ToList<OmeuTipo>();
                return result;
            }
            catch(Exception e){
                //todo: write good log
                Console.WriteLine(e);
            }
            return null;
        }

        public static string XmlDataDtoToString(DataDTO data){
            return "<?xml version='1.0' encoding='utf-16'?><data>" + data.content + "</data>";
        }
    }

    public class OmeuTipo{
        public string Endpoint { get; set; }
        public int Count { get; set; }

        public OmeuTipo(string endpoint, int count){
            Endpoint = endpoint;
            Count = count;
        }
    }
}