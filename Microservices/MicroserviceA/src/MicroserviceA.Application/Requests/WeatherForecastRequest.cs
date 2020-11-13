using System.Collections;
using System.Collections.Generic;
using MediatR;
using MicroserviceA.Application.DTOs;

namespace MicroserviceA.Application.Requests
{
    public class WeatherForecastRequest : IRequest<List<WeatherForecastDTO>>
    {
    }
}
