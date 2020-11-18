using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroserviceC.Application.Consumers
{
    public class SimpleMessageConsumer : IConsumer<ISimpleMessage>
    {
        ILogger<SimpleMessageConsumer> logger;

        public SimpleMessageConsumer(ILogger<SimpleMessageConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ISimpleMessage> context)
        {
            logger.LogInformation("Message: {0} CreationDateTime: {1}", context.Message.Message, context.Message.CreationDateTime);
        }
    }
}