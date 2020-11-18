using System;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceA.Business
{
    public class SimpleMessage : ISimpleMessage
    {
        public SimpleMessage()
        {
            CreationDateTime = DateTime.Now;
        }

        public DateTime CreationDateTime { get; set; }
        public string Message { get; set; }
    }
}
