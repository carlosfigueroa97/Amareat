using System;
using Amareat.Services.Messaging.Interfaces;
using Xamarin.Forms;

namespace Amareat.Services.Messaging.Implementations
{
    public class ApplicationMessage : IMessagingService
    {
        public void Send<TMessage>(TMessage message, object sender = null) where TMessage : ApplicationMessage
        {
            if (sender == null)
            {
                sender = new object();
            }

            MessagingCenter.Send(sender, typeof(TMessage).FullName, message);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : ApplicationMessage
        {
            MessagingCenter.Subscribe(subscriber, typeof(TMessage).FullName, callback, null);
        }

        public void Unsubscribe<TMessage>(object subscriber) where TMessage : ApplicationMessage
        {
            MessagingCenter.Unsubscribe<object, TMessage>(subscriber, typeof(TMessage).FullName);
        }
    }
}
