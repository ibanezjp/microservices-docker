

namespace MicroserviceA.Application.Requests
{
    using System;
    using MediatR;
    using System.Collections.Generic;
    using System.Text;
    using MicroserviceA.Application.DTOs;

    public  class RabbitMQRequest : IRequest<ResponseDTO>
    {
        public string Message { get; set; }
    }
}
