using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceB.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQlistenerController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private static readonly string[] Summaries = new[]
        {
            "RabbitMQ listener started"
        };

        private readonly ILogger<RabbitMQlistenerController> _logger;

        public RabbitMQlistenerController(ILogger<RabbitMQlistenerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Response> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 1).Select(index => new Response
            {
                Message = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
