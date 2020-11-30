using GreenPipes;
using MassTransit;
using MassTransit.Definition;

namespace MicroserviceD.Application.StatesMachines
{
    public class OrderStateMachineDefinition : SagaDefinition<OrderState>
    {
        public OrderStateMachineDefinition()
        {
            //EndpointName = "order-state-custom-name";
        }
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(x => x.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
