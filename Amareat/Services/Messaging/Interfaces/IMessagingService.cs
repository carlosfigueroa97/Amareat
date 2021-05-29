using System;
using Amareat.Services.Messaging.Implementations;

namespace Amareat.Services.Messaging.Interfaces
{
    public interface IMessagingService
    {
        void Send<TMessage>(TMessage message, object sender = null) where TMessage : ApplicationMessage;

        void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : ApplicationMessage;

        void Unsubscribe<TMessage>(object subscriber) where TMessage : ApplicationMessage;
    }
}
