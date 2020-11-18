using System;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceB.Business
{
    public class SimpleMessage : ISimpleMessage
    {
        public SimpleMessage()
        {
        }

        public DateTime CreationDateTime { get; set; }
        public string Message { get; set; }
    }
}
