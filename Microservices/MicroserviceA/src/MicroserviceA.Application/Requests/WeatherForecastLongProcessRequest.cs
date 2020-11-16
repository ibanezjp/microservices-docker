using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace MicroserviceA.Application.Requests
{
    public class WeatherForecastLongProcessRequest : IRequest<Guid>
    {
    }
}
