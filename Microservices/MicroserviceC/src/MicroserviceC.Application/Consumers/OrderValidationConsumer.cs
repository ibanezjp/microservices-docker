using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceC.Application.Consumers
{
    public class OrderValidationConsumer : IConsumer<IRequestValidateOrder>
    {
        public OrderValidationConsumer()
        {
            
        }

        public async Task Consume(ConsumeContext<IRequestValidateOrder> context)
        {
            await context.Publish<IOrderValidationCompleted>(new
            {
                context.Message.OrderId,
                Approved = context.Message.Amount <= 1000,
                Reason = context.Message.Amount <= 1000 ? string.Empty : "Insufficient Credit"
            });
        }
    }
}
