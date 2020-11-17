using Microservice.Common.EventBus.Events.Base;
using System;

namespace Microservice.Common.EventBus.Events
{
    public class LongProcessEvent : EventBusEvent
    {
        public Guid MessageId { get; set; }
        public string Message { get; set; }
    }
}
