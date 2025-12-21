using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using LightKit.Extensions;

namespace LightKit.Ava;

public static class DragDropCommand
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

    static DragDropCommand()
    {
        CommandProperty.Changed.AddClassHandler<Control>(HandleCommandChanged);
    }

    static void HandleCommandChanged(Control control, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is ICommand)
        {
            control.EnsureBackground();

            DragDrop.SetAllowDrop(control, true);
            control.AddHandler(DragDrop.DropEvent, OnDrop);
        }
        else
        {
            DragDrop.SetAllowDrop(control, false);
            control.RemoveHandler(DragDrop.DropEvent, OnDrop);
        }
    }

    static void OnDrop(object? sender, DragEventArgs e)
    {
        if (sender is not Control control)
            return;

        var command = GetCommand(control);
        if (command == null || !command.CanExecute(e))
            return;

        command.Execute(e);
        e.Handled = true;
    }
}
