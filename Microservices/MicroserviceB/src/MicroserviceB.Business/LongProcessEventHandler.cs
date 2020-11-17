using System;
using System.Threading.Tasks;
using Microservice.Common.EventBus;
using Microservice.Common.EventBus.Events;

namespace MicroserviceB.Business
{
    public class LongProcessEventHandler : IEventBusHandler<LongProcessEvent>
    {
        public LongProcessEventHandler()
        {

        }

        public Task Handle(LongProcessEvent eventBusEvent)
        {
            Console.WriteLine($"New Message : Message Id={eventBusEvent.MessageId}, Message={eventBusEvent.Message} ");
            return Task.CompletedTask;
        }
    }
}
