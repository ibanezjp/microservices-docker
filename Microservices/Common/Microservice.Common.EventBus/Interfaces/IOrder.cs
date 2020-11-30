using System;
using System.Dynamic;

namespace Microservice.Common.EventBus.Interfaces
{
    public interface IOrderAccepted
    {
        Guid OrderId { get; }
        string Status { get; }
        DateTime CreationDateTime { get; }
        double Amount { get; }
    }

    public interface IOrderAcceptedEvent
    {
        Guid OrderId { get; }
        DateTime CreationDateTime { get; }
        double Amount { get; }
    }

    public interface IOrderRejected
    {
        Guid OrderId { get; }
        string Reason { get; }
    }

    public interface ISubmitOrder
    {
        Guid OrderId { get; }
        double Amount { get; }
    }
    public interface ICheckOrderState
    {
        Guid OrderId { get; }
    }

    public interface IOrderCreated
    {
        double Amount { get; }
        Guid OrderId { get; }
        string Status { get; }
        DateTime CreationDateTime { get; set; }
    }

    public interface IOrderState
    {
        Guid OrderId { get; }
        string State { get; }
    }

    public interface IOrderNotFound
    {
        Guid OrderId { get; }
    }

    public interface IRequestValidateOrder
    {
        Guid OrderId { get; }
        double Amount { get; }
    }

    public interface IOrderValidationCompleted
    {
        Guid OrderId { get; }
        bool Approved { get; }
        string Reason { get; }
    }

    public interface IOrderValidationApproved
    {
        Guid OrderId { get; }
    }

    public interface IOrderValidationRejected
    {
        Guid OrderId { get; }
        string Reason { get; }
    }
}
