using System;
using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace MicroserviceC.Application.Consumers
{
    public class RemoteSimpleMessageConsumer : IConsumer<IRemoteSimpleMessageRequest>
    {
        ILogger<RemoteSimpleMessageConsumer> logger;

        public RemoteSimpleMessageConsumer(ILogger<RemoteSimpleMessageConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<IRemoteSimpleMessageRequest> context)
        {
            await context.RespondAsync<IRemoteSimpleMessageResponse>(new
            {
                Message = "Response from Microservice C",
                CreationDateTime = InVar.Timestamp
            });
        }
    }
}