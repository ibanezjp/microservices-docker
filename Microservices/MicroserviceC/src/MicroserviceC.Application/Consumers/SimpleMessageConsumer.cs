using System;
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
            var retryAttempt = context.GetRetryAttempt();
            var retryCount = context.GetRetryCount();
            var redeliveryCount = context.GetRedeliveryCount();

            logger.LogInformation("Message: {0} CreationDateTime: {1}", context.Message.Message, context.Message.CreationDateTime);
            logger.LogInformation("RetryAttempt: {0} RetryCount: {1} RedeliveryCount: {2}", retryAttempt, retryCount, redeliveryCount);

            if (context.Message.Message.Contains("error", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApplicationException("Invalid message content!");
            }
        }
    }
}