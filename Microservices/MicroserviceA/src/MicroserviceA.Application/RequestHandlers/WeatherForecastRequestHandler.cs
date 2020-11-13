using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroserviceA.Application.DTOs;
using MicroserviceA.Application.Requests;

namespace MicroserviceA.Application.RequestHandlers
{
    public class WeatherForecastRequestHandler : IRequestHandler<WeatherForecastRequest, List<WeatherForecastDTO>>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<List<WeatherForecastDTO>> Handle(WeatherForecastRequest request, CancellationToken cancellationToken)
        {
            var rng = new Random();
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecastDTO
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();

            return Task.FromResult(data);
        }
    }
}
