using System.Collections.ObjectModel;
using System.Reactive.Linq;
using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;
using AirKit.Reactive.Interfaces;
using AirKit.Reactive.Logging;
using DynamicData;

namespace AirKit.Reactive.Demo.Views;

[View<LogView>]
sealed class LogView_Model : ViewModel, ILogger
{
    readonly SourceList<LogModel> _logs = new();

    public ReadOnlyObservableCollection<LogModel> Logs { get; }

    public LogView_Model()
    {
        _logs.Connect()
            .Synchronize()
            .ObserveOn(AvaloniaScheduler.Instance)
            .Bind(out var items)
            .Subscribe();
        Logs = items;
        Log.AddLogger(this);
    }

    public void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues)
    {
        var message = Log.FormatMessage(messageTemplate, propertyValues);
        _logs.Add(new(DateTimeOffset.Now, level, area, message));
    }
}
