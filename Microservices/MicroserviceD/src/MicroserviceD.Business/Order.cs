using System;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceD.Business
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string Status { get; set;  }
        public DateTime CreationDateTime { get; set; }
    }
}
