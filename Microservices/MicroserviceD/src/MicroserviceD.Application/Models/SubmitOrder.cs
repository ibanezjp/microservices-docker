using System;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceD.Application.Models
{
    public class SubmitOrder : ISubmitOrder
    {
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
    }
}
