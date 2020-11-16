using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microservice.Common.EventBus.Commands;
using Microservice.Common.EventBus.Events;
using Microservice.Common.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Microservice.Common.EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IMediator mediator;
        private readonly Dictionary<string, List<Type>> handlers;
        private readonly List<Type> eventTypes;

        public RabbitMQEventBus(IMediator mediator)
        {
            this.mediator = mediator;
            handlers = new Dictionary<string, List<Type>>();
            eventTypes = new List<Type>();
        }


        //public Task EnviarComando<T>(T comando) where T : Comando
        //{
        //    return _mediator.Send(comando);
        //}

        //public void Publish<T>(T evento) where T : Evento
        //{
        //    var factory = new ConnectionFactory() { HostName = "rabbit-vaxi-web" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        var eventName = evento.GetType().Name;

        //        channel.QueueDeclare(eventName, false, false, false, null);

        //        var message = JsonConvert.SerializeObject(evento);
        //        var body = Encoding.UTF8.GetBytes(message);
        //        channel.BasicPublish("", eventName, null, body);
        //    }


        //}

        //public void Subscribe<T, TH>()
        //    where T : Evento
        //    where TH : IEventoManejador<T>
        //{
        //    var eventoNombre = typeof(T).Name;
        //    var manejadorEventoTipo = typeof(TH);

        //    if (!_eventoTipos.Contains(typeof(T)))
        //    {
        //        _eventoTipos.Add(typeof(T));
        //    }

        //    if (!_manejadores.ContainsKey(eventoNombre))
        //    {
        //        _manejadores.Add(eventoNombre, new List<Type>());
        //    }

        //    if (_manejadores[eventoNombre].Any(x => x.GetType() == manejadorEventoTipo))
        //    {
        //        throw new ArgumentException($"El manejador {manejadorEventoTipo.Name} fue registrado anteriormente por {eventoNombre}");
        //    }

        //    _manejadores[eventoNombre].Add(manejadorEventoTipo);

        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "rabbit-vaxi-web",
        //        DispatchConsumersAsync = true
        //    };

        //    var connection = factory.CreateConnection();
        //    var channel = connection.CreateModel();


        //    channel.QueueDeclare(eventoNombre, false, false, false, null);

        //    var consumer = new AsyncEventingBasicConsumer(channel);
        //    consumer.Received += Consumer_Delegate;

        //    channel.BasicConsume(eventoNombre, true, consumer);

        //}

        //private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        //{
        //    var nombreEvento = e.RoutingKey;
        //    var message = Encoding.UTF8.GetString(e.Body.ToArray());

        //    try
        //    {
        //        if (_manejadores.ContainsKey(nombreEvento))
        //        {

        //            var subscriptions = _manejadores[nombreEvento];
        //            foreach (var sb in subscriptions)
        //            {
        //                var manejador = Activator.CreateInstance(sb);
        //                if (manejador == null) continue;

        //                var tipoEvento = _eventoTipos.SingleOrDefault(x => x.Name == nombreEvento);
        //                var eventoDS = JsonConvert.DeserializeObject(message, tipoEvento);

        //                var concretoTipo = typeof(IEventoManejador<>).MakeGenericType(tipoEvento);

        //                await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });

        //            }



        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //}
        public Task SendCommand<T>(T command) where T : EventBusCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(T eventBusEvent) where T : EventBusEvent
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq-server-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = eventBusEvent.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(eventBusEvent);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>() where T : EventBusEvent where TH : IEventBusHandler
        {
            throw new NotImplementedException();
        }
    }
}
