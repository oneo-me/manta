using System.Windows.Input;
using Avalonia;
using Avalonia.Input;

namespace Axe.Controls.Commands;

public static class DoubleTapped
{
    public static readonly AttachedProperty<ICommand?> CommandProperty =
        AvaloniaProperty.RegisterAttached<InputElement, InputElement, ICommand?>("Command");

    public static void SetCommand(InputElement obj, ICommand? value)
    {
        obj.SetValue(CommandProperty, value);
    }

    public static ICommand? GetCommand(InputElement obj)
    {
        return obj.GetValue(CommandProperty);
    }

    public static readonly AttachedProperty<object?> CommandParameterProperty =
        AvaloniaProperty.RegisterAttached<InputElement, InputElement, object?>("CommandParameter");

    public static void SetCommandParameter(InputElement obj, object? value)
    {
        obj.SetValue(CommandParameterProperty, value);
    }

    public static object? GetCommandParameter(InputElement obj)
    {
        return obj.GetValue(CommandParameterProperty);
    }

    static DoubleTapped()
    {
        CommandProperty.Changed.AddClassHandler<InputElement>(HandleCommandChanged);
    }

    static void HandleCommandChanged(InputElement inputElement, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is ICommand)
            inputElement.DoubleTapped += Handler;
        else
            inputElement.DoubleTapped -= Handler;
    }

    static void Handler(object? sender, TappedEventArgs e)
    {
        if (sender is not InputElement inputElement)
            return;

        var command = GetCommand(inputElement);
        if (command is null)
            return;

        var commandParameter = inputElement.GetValue(CommandParameterProperty);

        if (command.CanExecute(commandParameter))
            command.Execute(commandParameter);
    }
}
