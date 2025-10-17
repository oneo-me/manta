using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace LightKit.Avalonia;

public static class DoubleTappedCommand
{
    public static readonly AttachedProperty<ICommand?> CommandProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, ICommand?>("Command");

    public static void SetCommand(Control obj, ICommand? value)
    {
        obj.SetValue(CommandProperty, value);
    }

    public static ICommand? GetCommand(Control obj)
    {
        return obj.GetValue(CommandProperty);
    }

    public static readonly AttachedProperty<object?> CommandParameterProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, object?>("CommandParameter");

    public static void SetCommandParameter(Control obj, object? value)
    {
        obj.SetValue(CommandParameterProperty, value);
    }

    public static object? GetCommandParameter(Control obj)
    {
        return obj.GetValue(CommandParameterProperty);
    }

    static DoubleTappedCommand()
    {
        CommandProperty.Changed.AddClassHandler<Control>(HandleCommandChanged);
    }

    static void HandleCommandChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is ICommand)
            control.AddHandler(InputElement.DoubleTappedEvent, OnDoubleTapped);
        else
            control.RemoveHandler(InputElement.DoubleTappedEvent, OnDoubleTapped);
    }

    static void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not Control control)
            return;

        var command = GetCommand(control);
        if (command is null)
            return;

        var commandParameter = control.GetValue(CommandParameterProperty);

        if (command.CanExecute(commandParameter))
            command.Execute(commandParameter);
    }
}
