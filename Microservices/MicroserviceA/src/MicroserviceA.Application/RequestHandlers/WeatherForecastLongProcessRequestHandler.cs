using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using MicroserviceA.Application.Requests;
using MicroserviceA.Business;

namespace MicroserviceA.Application.RequestHandlers
{
    public class WeatherForecastLongProcessRequestHandler : IRequestHandler<WeatherForecastLongProcessRequest, Guid>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public WeatherForecastLongProcessRequestHandler(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(WeatherForecastLongProcessRequest request, CancellationToken cancellationToken)
        {
            var simpleMessage = new SimpleMessage
            {
                Message = "Message published from Microservice A"
            };
            await publishEndpoint.Publish(simpleMessage, cancellationToken);

            return Guid.Empty;
        }
    }
}
