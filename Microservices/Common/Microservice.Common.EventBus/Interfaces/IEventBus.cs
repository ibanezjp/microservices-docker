using System;
using System.Threading.Tasks;
using Microservice.Common.EventBus;
using Microservice.Common.EventBus.Commands;
using Microservice.Common.EventBus.Events;

namespace Microservice.Common.Interfaces
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : EventBusCommand;

        void Publish<T>(T eventBusEvent) where T : EventBusEvent;

        void Subscribe<T, TH>() where T : EventBusEvent where TH : IEventBusHandler;
    }
}
