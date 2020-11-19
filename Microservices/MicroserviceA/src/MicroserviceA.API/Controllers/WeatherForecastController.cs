using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MicroserviceA.Application.DTOs;
using MicroserviceA.Application.Requests;
using MicroserviceA.Business;

namespace MicroserviceA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IMediator mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastDTO>> Get()
        {
            return await mediator.Send(new WeatherForecastRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SimpleMessageRequest simpleMessageRequest)
        {
            await mediator.Send(simpleMessageRequest);

            return Ok();
        }
    }
}
