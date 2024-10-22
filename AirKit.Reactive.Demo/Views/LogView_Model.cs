using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;
using AirKit.Reactive.Interfaces;
using AirKit.Reactive.Logging;
using ObservableCollections;

namespace AirKit.Reactive.Demo.Views;

[View<LogView>]
sealed class LogView_Model : ViewModel, ILogger
{
    readonly ObservableFixedSizeRingBuffer<LogModel> _logs = new(6);

    public NotifyCollectionChangedSynchronizedViewList<LogModel> Logs { get; }

    public LogView_Model()
    {
        Logs = _logs.ToNotifyCollectionChanged();
        Log.AddLogger(this);
    }

    public void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues)
    {
        var message = Log.FormatMessage(messageTemplate, propertyValues);
        _logs.AddLast(new(DateTimeOffset.Now, level, area, message));
    }
}
