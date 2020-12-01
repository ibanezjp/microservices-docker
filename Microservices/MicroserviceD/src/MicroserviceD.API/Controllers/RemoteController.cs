using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceD.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteController : ControllerBase
    {
        private readonly ILogger<RemoteController> logger;
        private readonly IRequestClient<IRemoteSimpleMessageRequest> requestClientRemoveSimpleMessage;
        private readonly IHttpClientFactory httpClientFactory;

        public RemoteController(
            ILogger<RemoteController> logger,
            IRequestClient<IRemoteSimpleMessageRequest> requestClientRemoveSimpleMessage,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.requestClientRemoveSimpleMessage = requestClientRemoveSimpleMessage;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await requestClientRemoveSimpleMessage.GetResponse<IRemoteSimpleMessageResponse>(new { });
            return Ok(response.Message);
        }

        [HttpGet]
        [Route("http")]
        public async Task<IActionResult> GetRemoteByHttp()
        {
            var httpClient = httpClientFactory.CreateClient("RemoteServiceByHttp");
            var httpResponseMessage = await httpClient.GetAsync("remote");

            if (!httpResponseMessage.IsSuccessStatusCode)
                return StatusCode((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync());

            var message = await httpResponseMessage.Content.ReadAsStringAsync();
            return Ok(message);

        }

        [HttpGet]
        [Route("gprc")]
        public async Task<IActionResult> GetRemoteByGPRC()
        {
            return Ok();
        }
    }
}
