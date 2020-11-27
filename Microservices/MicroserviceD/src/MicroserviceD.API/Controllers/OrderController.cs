using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MassTransit;
using Microservice.Common.EventBus.Interfaces;
using MicroserviceD.Application.Models;

namespace MicroserviceD.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> logger;
        private readonly IRequestClient<ISubmitOrder> requestClientSubmitOrder;
        private readonly IRequestClient<ICheckOrderState> requestClientCheckOrderState;

        public OrderController(
            ILogger<OrderController> logger, 
            IRequestClient<ISubmitOrder> requestClientSubmitOrder,
            IRequestClient<ICheckOrderState> requestClientCheckOrderState)
        {
            this.logger = logger;
            this.requestClientSubmitOrder = requestClientSubmitOrder;
            this.requestClientCheckOrderState = requestClientCheckOrderState;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubmitOrder submitOrder)
        {
            var (accepted, rejected) =
                await requestClientSubmitOrder.GetResponse<IOrderAccepted, IOrderRejected>(submitOrder);

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Accepted(response.Message);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message.Reason);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var (orderState, notFound) = await requestClientCheckOrderState.GetResponse<IOrderState, IOrderNotFound>(new { OrderId = id });

            if (orderState.IsCompletedSuccessfully)
            {
                var response = await orderState;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }


        //private readonly IMediator mediator;

        //public OrderController(ILogger<OrderController> logger, IMediator mediator)
        //{
        //    this.logger = logger;
        //    this.mediator = mediator;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post(SubmitOrder submitOrder)
        //{
        //    var client = mediator.CreateRequestClient<ISubmitOrder>();
        //    var (accepted, rejected) = await client.GetResponse<IOrderAccepted, IOrderRejected>(submitOrder);
        //    if (accepted.IsCompletedSuccessfully)
        //    {
        //        var response = await accepted;
        //        return Accepted(response.Message);
        //    }
        //    else
        //    {
        //        var response = await rejected;
        //        return BadRequest(response.Message.Reason);
        //    }
        //}
    }
}
