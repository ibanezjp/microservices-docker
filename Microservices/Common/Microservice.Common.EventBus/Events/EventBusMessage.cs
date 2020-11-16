using MediatR;

namespace Microservice.Common.EventBus.Events
{
    public abstract class EventBusMessage : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        protected EventBusMessage()
        {
            MessageType = GetType().Name;
        }
    }
}
