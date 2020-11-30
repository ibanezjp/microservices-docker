using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace MicroserviceD.Application.Consumers
{
    public class SubmitOrderConsumerDefinition : ConsumerDefinition<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerDefinition()
        {
            
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
        {
            
        }
    }
}
