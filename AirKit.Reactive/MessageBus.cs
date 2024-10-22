using AirKit.Reactive.Logging;
using R3;

namespace AirKit.Reactive;

public static class MessageBus
{
    static readonly Subject<object?> _messageStream = new();

    public static Observable<T> Listen<T>()
    {
        Log.Verbose("MessageBus", "Listen {Type}", typeof(T).Name);

        return _messageStream
            .Synchronize()
            .Where(message => message is T)
            .Select(message => (T)message!);
    }

    public static void SendMessage<T>(T message)
    {
        Log.Verbose("MessageBus", "SendMessage {} to {Type}", message, typeof(T).Name);

        _messageStream.OnNext(message);
    }
}
