using R3;

namespace AirKit.Reactive;

public static class MessageBus
{
    static readonly Subject<object?> _messageStream = new();

    public static Observable<T> Listen<T>()
    {
        return _messageStream
            .Synchronize()
            .Where(message => message is T)
            .Select(message => (T)message!);
    }

    public static void SendMessage<T>(T message)
    {
        _messageStream.OnNext(message);
    }
}
