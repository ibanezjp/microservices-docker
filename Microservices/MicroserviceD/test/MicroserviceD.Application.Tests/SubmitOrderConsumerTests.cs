using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using Microservice.Common.EventBus.Interfaces;
using MicroserviceD.Application.Consumers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace MicroserviceD.Application.Tests
{
    [TestClass]
    public class SubmitOrderConsumerTests
    {
        [TestMethod]
        public async Task SubmitOrderConsumer_Accepted()
        {
            var harness = new InMemoryTestHarness();
            harness.Consumer<SubmitOrderConsumer>();

            try
            {
                await harness.Start();

                var orderId = NewId.NextGuid();

                var requestClientSubmitOrder = await harness.ConnectRequestClient<ISubmitOrder>();

                var (accepted, rejected) =
                    await requestClientSubmitOrder.GetResponse<IOrderAccepted, IOrderRejected>(new
                    {
                        OrderId = orderId,
                        Amount = 900
                    });

                harness.Consumed.Select<ISubmitOrder>().Any().Should().Be.True();
                accepted.IsCompletedSuccessfully.Should().Be.True();
                rejected.IsCompletedSuccessfully.Should().Be.False();

                harness.Published.Select<IOrderAcceptedEvent>().Any().Should().Be.True();
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
