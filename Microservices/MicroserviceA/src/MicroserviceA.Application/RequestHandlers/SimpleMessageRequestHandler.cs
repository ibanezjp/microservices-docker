using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using MicroserviceA.Application.Requests;
using MicroserviceA.Business;

namespace MicroserviceA.Application.RequestHandlers
{
    public class SimpleMessageRequestHandler : IRequestHandler<SimpleMessageRequest, Unit>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public SimpleMessageRequestHandler(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(SimpleMessageRequest simpleMessageRequest, CancellationToken cancellationToken)
        {
            var simpleMessage = new SimpleMessage
            {
                Message = simpleMessageRequest.Message
            };
            await publishEndpoint.Publish(simpleMessage, cancellationToken);

            return Unit.Value;
        }
    }
}
