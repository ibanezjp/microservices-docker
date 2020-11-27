using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroserviceD.Application.Consumers
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> logger;
        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            this.logger = logger;

        }
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            if (context.RequestId.HasValue)
            {
                if (context.Message.Amount > 0)
                {
                    await context.Publish<IOrderAccepted>(new
                    {
                        context.Message.Amount,
                        context.Message.OrderId,
                        CreationDateTime = InVar.Timestamp,
                        Status = "Created"
                    });

                    await context.RespondAsync<IOrderAccepted>(new
                    {
                        context.Message.Amount,
                        context.Message.OrderId,
                        CreationDateTime = InVar.Timestamp,
                        Status = "Created"
                    });
                }
                else
                    await context.RespondAsync<IOrderRejected>(new
                    {
                        context.Message.OrderId,
                        Reason = "Amount should be greater than $0"
                    });
            }
        }
    }
}
