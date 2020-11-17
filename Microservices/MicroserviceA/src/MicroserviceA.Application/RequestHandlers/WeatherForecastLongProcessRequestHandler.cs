using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservice.Common.EventBus.Events;
using Microservice.Common.Interfaces;
using MicroserviceA.Application.Requests;

namespace MicroserviceA.Application.RequestHandlers
{
    public class WeatherForecastLongProcessRequestHandler : IRequestHandler<RabbitMQAddRequest, Guid>
    {
        private IEventBus eventBus;
        public WeatherForecastLongProcessRequestHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public Task<Guid> Handle(RabbitMQAddRequest request, CancellationToken cancellationToken)
        {
            var messageid = Guid.NewGuid();
            eventBus.Publish(new LongProcessEvent { MessageId = messageid, Message = request.Message });
            return Task.FromResult(messageid);
        }
    }
}
