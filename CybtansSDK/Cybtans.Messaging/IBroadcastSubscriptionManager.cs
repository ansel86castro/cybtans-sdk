namespace Cybtans.Messaging
{
    public interface IBroadcastSubscriptionManager
    {
        void Subscribe<TMessage, THandler>(string? channel = null) where THandler : IMessageHandler<TMessage>;
        void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? channel = null);
        void Unsubscribe<TMessage>(string? channel = null);
    }
}
