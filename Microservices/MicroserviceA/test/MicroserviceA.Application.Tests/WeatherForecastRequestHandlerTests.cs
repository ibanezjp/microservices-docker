using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroserviceA.Application.RequestHandlers;
using MicroserviceA.Application.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace MicroserviceA.Application.Tests
{
    [TestClass]
    public class WeatherForecastRequestHandlerTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var request = new WeatherForecastRequest();
            var handler = new WeatherForecastRequestHandler();
            var response = await handler.Handle(request, new CancellationToken());
            response.Count.Should().Be.EqualTo(5);
        }
    }
}
