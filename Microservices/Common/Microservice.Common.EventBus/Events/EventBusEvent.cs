using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Common.EventBus.Events
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
