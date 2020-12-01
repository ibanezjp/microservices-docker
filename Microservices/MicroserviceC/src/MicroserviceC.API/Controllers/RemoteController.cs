using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteController : ControllerBase
    {
        private readonly ILogger<RemoteController> logger;
        private readonly IRequestClient<IRemoteSimpleMessageRequest> requestClientRemoveSimpleMessage;

        public RemoteController(
            ILogger<RemoteController> logger,
            IRequestClient<IRemoteSimpleMessageRequest> requestClientRemoveSimpleMessage)
        {
            this.logger = logger;
            this.requestClientRemoveSimpleMessage = requestClientRemoveSimpleMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await requestClientRemoveSimpleMessage.GetResponse<IRemoteSimpleMessageResponse>(new {});
            return Ok(response.Message);
        }
    }
}
