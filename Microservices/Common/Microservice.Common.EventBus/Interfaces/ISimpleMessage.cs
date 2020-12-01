using System;

namespace Microservice.Common.EventBus.Interfaces
{
    public interface ISimpleMessage
    {
        DateTime CreationDateTime { get; }
        string Message { get; }
    }

    public interface IRemoteSimpleMessageRequest
    {

    }

    public interface IRemoteSimpleMessageResponse
    {
        string Message { get; set; }
        DateTime CreationDateTime { get; }
    }
}
