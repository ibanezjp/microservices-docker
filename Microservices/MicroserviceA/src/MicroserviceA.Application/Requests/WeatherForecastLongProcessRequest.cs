using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace MicroserviceA.Application.Requests
{
    public class RabbitMQAddRequest : IRequest<Guid>
    {
        public string Message { get; set; }
    }
}
