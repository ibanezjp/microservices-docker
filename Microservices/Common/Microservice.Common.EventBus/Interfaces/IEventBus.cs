using Microservice.Common.EventBus;
using Microservice.Common.EventBus.Events.Base;

namespace Microservice.Common.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T eventBusEvent) where T : EventBusEvent;

        void Subscribe<T, TH>() where T : EventBusEvent where TH : IEventBusHandler;
    }
}
