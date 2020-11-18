using System;

namespace Microservice.Common.EventBus.Interfaces
{
    public interface ISimpleMessage
    {
        DateTime CreationDateTime { get; }
        string Message { get; }
    }
}
