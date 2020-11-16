using System.Threading.Tasks;
using Microservice.Common.EventBus.Events;

namespace Microservice.Common.EventBus
{
    public interface IEventBusHandler<in TEvent> : IEventBusHandler where TEvent : EventBusEvent
    {
        Task Handle(TEvent eventBusEvent);
    }

    public interface IEventBusHandler
    {

    }
}
