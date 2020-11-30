using System;
using Automatonymous;
using MassTransit;
using MassTransit.Saga;
using Microservice.Common.EventBus.Interfaces;

namespace MicroserviceD.Application.StatesMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Event(() => OrderAcceptedEvent, x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => OrderStateRequested, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue)
                    {
                        await context.RespondAsync<IOrderNotFound>(new { context.Message.OrderId });
                    }
                }));
            });

            Event(() => OrderValidationApproved, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderValidationRejected, x => x.CorrelateById(m => m.Message.OrderId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderAcceptedEvent)
                    .Then(context =>
                    {
                        context.Instance.Amount ??= context.Data.Amount;
                        context.Instance.CreationDateTime ??= context.Data.CreationDateTime;
                    })
                    .SendAsync(new Uri("queue:order-validation"),context => context.Init<IRequestValidateOrder>(new
                    {
                        OrderId = context.Instance.CorrelationId,
                        context.Instance.Amount
                    }))
                    .TransitionTo(Created));

            During(Created,
                Ignore(OrderAcceptedEvent),
                When(OrderValidationApproved)
                    .Then(context =>
                    {
                        context.Instance.Approved ??= true;
                    })
                    .TransitionTo(Approved),
                When(OrderValidationRejected)
                    .Then(context =>
                    {
                        context.Instance.Approved ??= false;
                        context.Instance.RejectedReason ??= context.Data.Reason;
                    })
                    .TransitionTo(Rejected));


            DuringAny(
                When(OrderStateRequested)
                    .RespondAsync(x => x.Init<IOrderState>(new
                    {
                        OrderId = x.Instance.CorrelationId,
                        State = x.Instance.CurrentState
                    })));

        }

        public State Created { get; private set; }
        public State Rejected { get; private set; }
        public State Approved { get; private set; }
        public State Processing { get; private set; }

        public Event<IOrderAcceptedEvent> OrderAcceptedEvent { get; set; }
        public Event<ICheckOrderState> OrderStateRequested { get; set; }
        public Event<IOrderValidationApproved> OrderValidationApproved { get; set; }
        public Event<IOrderValidationRejected> OrderValidationRejected { get; set; }
    }

    public class OrderState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int Version { get; set; }
        public double? Amount { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public bool? Approved { get; set; }
        public string RejectedReason { get; set; }
    }
}
