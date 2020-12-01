using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroserviceD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollyController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public PollyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("remote");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var temperature = await httpResponseMessage.Content.ReadAsStringAsync();
                return Ok(temperature);
            }

            return StatusCode((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
