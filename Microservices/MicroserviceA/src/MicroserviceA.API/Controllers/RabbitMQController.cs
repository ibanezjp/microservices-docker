using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MicroserviceA.Application.DTOs;
using MicroserviceA.Application.Requests;

namespace MicroserviceA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQController : ControllerBase
    {
        private readonly ILogger<RabbitMQController> logger;
        private readonly IMediator mediator;

        public RabbitMQController(ILogger<RabbitMQController> logger, IMediator mediator)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        //[HttpGet]
        //public async Task<IEnumerable<WeatherForecastDTO>> Get()
        //{
        //    return await mediator.Send(new WeatherForecastRequest());
        //}

        [HttpPost]
        public async Task<Guid> PostAPostProcess(RabbitMQAddRequest request)
        {
            return await mediator.Send(new RabbitMQAddRequest {Message = request.Message });
        }
    }
}
