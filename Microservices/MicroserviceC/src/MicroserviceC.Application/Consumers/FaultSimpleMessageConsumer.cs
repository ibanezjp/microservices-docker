using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroserviceC.Application.Consumers
{
    public class FaultSimpleMessageConsumer : IConsumer<Fault<ISimpleMessage>>
    {
        ILogger<FaultSimpleMessageConsumer> logger;

        public FaultSimpleMessageConsumer(ILogger<FaultSimpleMessageConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<Fault<ISimpleMessage>> context)
        {
            logger.LogInformation("Exception: {0}", context.Message.Exceptions.First().ExceptionType);
        }
    }
}