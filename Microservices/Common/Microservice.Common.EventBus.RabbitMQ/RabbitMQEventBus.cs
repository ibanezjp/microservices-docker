using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservice.Common.EventBus.Events.Base;
using Microservice.Common.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Common.EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly Dictionary<string, List<Type>> handlers;
        private readonly List<Type> eventTypes;

        public RabbitMQEventBus()
        {
            handlers = new Dictionary<string, List<Type>>();
            eventTypes = new List<Type>();
        }

        public void Publish<T>(T eventBusEvent) where T : EventBusEvent
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq-server-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = eventBusEvent.GetType().Name;

                channel.QueueDeclare(eventName, true, false, false, null);

                var message = JsonConvert.SerializeObject(eventBusEvent);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>() where T : EventBusEvent where TH : IEventBusHandler
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!eventTypes.Contains(typeof(T)))
            {
                eventTypes.Add(typeof(T));
            }

            if (!handlers.ContainsKey(eventName))
            {
                handlers.Add(eventName, new List<Type>());
            }

            if (handlers[eventName].Any(x => x.GetType() == handlerType))
            {
                throw new ArgumentException($"El manejador {handlerType.Name} fue registrado anteriormente por {eventName}");
            }

            handlers[eventName].Add(handlerType);

            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq-server-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();


            channel.QueueDeclare(eventName, true, false, false, null);

            var asyncEventingBasicConsumer = new AsyncEventingBasicConsumer(channel);
            asyncEventingBasicConsumer.Received += AsyncEventingBasicConsumer_Received; ;

            channel.BasicConsume(eventName, true, asyncEventingBasicConsumer);
        }

        private async Task AsyncEventingBasicConsumer_Received(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var eventName = basicDeliverEventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(basicDeliverEventArgs.Body.ToArray());

            try
            {
                if (handlers.ContainsKey(eventName))
                {

                    var subscriptions = handlers[eventName];
                    foreach (var sb in subscriptions)
                    {
                        var handler = Activator.CreateInstance(sb);
                        if (handler == null) continue;

                        var eventType = eventTypes.SingleOrDefault(x => x.Name == eventName);
                        var eventBusEvent = JsonConvert.DeserializeObject(message, eventType);

                        var type = typeof(IEventBusHandler<>).MakeGenericType(eventType);

                        await(Task)type.GetMethod("Handle").Invoke(handler, new[] { eventBusEvent });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
