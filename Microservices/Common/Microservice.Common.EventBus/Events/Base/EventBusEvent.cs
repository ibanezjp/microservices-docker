using System;

namespace Microservice.Common.EventBus.Events.Base
{
    public abstract class EventBusEvent
    {
        public DateTime Timestamp { get; protected set; }
        protected EventBusEvent()
        {
            Timestamp = DateTime.Now;
        }
    }
}
