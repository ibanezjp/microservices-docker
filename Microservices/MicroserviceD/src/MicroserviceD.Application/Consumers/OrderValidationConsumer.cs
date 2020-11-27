using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroserviceD.Application.Consumers
{
    public class OrderValidationConsumer : IConsumer<IOrderValidationCompleted>
    {
        private readonly ILogger<IOrderValidationCompleted> logger;
        public OrderValidationConsumer(ILogger<IOrderValidationCompleted> logger)
        {
            this.logger = logger;

        }
        public async Task Consume(ConsumeContext<IOrderValidationCompleted> context)
        {
            if (context.Message.Approved)
            {
                await context.Publish<IOrderValidationApproved>(new
                {
                    context.Message.OrderId
                });
            }
            else
            {
                await context.Publish<IOrderValidationRejected>(new
                {
                    context.Message.OrderId,
                    context.Message.Reason
                });
            }
        }
    }
}
