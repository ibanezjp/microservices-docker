using System;
using Microservice.Common.EventBus.Events;

namespace Microservice.Common.EventBus.Commands
{
    public abstract class EventBusCommand : EventBusMessage
    {
        public DateTime Timestamp { get; protected set; }

        protected EventBusCommand()
        {
            Timestamp = DateTime.Now;
        }
    }
}
