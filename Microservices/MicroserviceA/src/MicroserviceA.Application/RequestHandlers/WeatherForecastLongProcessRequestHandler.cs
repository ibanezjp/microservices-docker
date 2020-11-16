using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservice.Common.EventBus.Events;
using Microservice.Common.Interfaces;
using MicroserviceA.Application.Requests;

namespace MicroserviceA.Application.RequestHandlers
{
    public class WeatherForecastLongProcessRequestHandler : IRequestHandler<WeatherForecastLongProcessRequest, Guid>
    {
        private IEventBus eventBus;
        public WeatherForecastLongProcessRequestHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public Task<Guid> Handle(WeatherForecastLongProcessRequest request, CancellationToken cancellationToken)
        {
            eventBus.Publish(new LongProcessEvent());
            return Task.FromResult(Guid.Empty);
        }
    }
}
