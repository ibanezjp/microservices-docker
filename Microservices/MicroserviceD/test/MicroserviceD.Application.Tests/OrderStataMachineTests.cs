using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using Microservice.Common.EventBus.Interfaces;
using MicroserviceD.Application.Consumers;
using MicroserviceD.Application.StatesMachines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;

namespace MicroserviceD.Application.Tests
{
    [TestClass]
    public class OrderStataMachineTests
    {
        [TestMethod]
        public async Task SubmitOrderConsumer_Accepted()
        {
            var orderStateMachine = new OrderStateMachine();
            var harness = new InMemoryTestHarness();

            var saga = harness.StateMachineSaga<OrderState, OrderStateMachine>(orderStateMachine);


            try
            {
                await harness.Start();

                var orderId = NewId.NextGuid();

                await harness.Bus.Publish<IOrderAcceptedEvent>(new
                {
                    OrderId = orderId,
                    CreationDateTime = InVar.Timestamp,
                    Amount = 900
                });

                saga.Created.Select(x => x.CorrelationId == orderId).Any().Should().Be.True();

                var correlationId = await saga.Exists(orderId, x => x.Created);

                correlationId.HasValue.Should().Be.True();

                var instance = saga.Sagas.Contains(correlationId.Value);

                instance.CurrentState.Should().Be.EqualTo("Created");
                instance.CorrelationId.Should().Be.EqualTo(orderId);
                instance.Amount.Should().Be.EqualTo(900);

                harness.Sent.Select<IRequestValidateOrder>().Any().Should().Be.True();
                harness.Published.Select<IRequestValidateOrder>().Any().Should().Be.False();

            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
