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
            return Task.CompletedTask;
        }
    }
}
