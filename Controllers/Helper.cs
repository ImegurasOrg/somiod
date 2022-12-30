using MQTTnet;
using MQTTnet.Client;
using somiod.DAL;
using somiod.Models;
using Microsoft.EntityFrameworkCore;

namespace somiod.Controllers{
    public class Helper{

        private static InheritanceMappingContext _context;

        public Helper(InheritanceMappingContext context){
            _context = context;
        }
        public static async Task PublishAsync(string connUri, string topic, string message){
            var mqttFactory = new MqttFactory();
            using( var mqttClient = mqttFactory.CreateMqttClient()){
                var options = new MqttClientOptionsBuilder().WithConnectionUri(connUri).Build();
                try {
                    using (var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(1))){
                        await mqttClient.ConnectAsync(options, timeoutToken.Token);
                    }
                }
                catch (Exception e){
                    Console.WriteLine(e);
                    return;
                }
                var payload = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(message).Build();
                await mqttClient.PublishAsync(payload);
                await mqttClient.DisconnectAsync();
            }
        }

        public static List<OmeuTipo>? CheckSubscritions(string module){
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