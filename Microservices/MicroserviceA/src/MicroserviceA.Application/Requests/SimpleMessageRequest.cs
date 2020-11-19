using MediatR;

namespace MicroserviceA.Application.Requests
{
    public class SimpleMessageRequest : IRequest<Unit>
    {
        public string Message { get; set; }
    }
}
