using System.Windows.Input;
using Avalonia;
using Manta.Controls;

namespace Manta.DevTools.Commands;

public class PointerCommands
{
    public static readonly AttachedProperty<ICommand?> EnterCommandProperty =
        AvaloniaProperty.RegisterAttached<PointerCommands, MTreeItem, ICommand?>("EnterCommand");

    public static void SetEnterCommand(MTreeItem obj, ICommand? value)
    {
        obj.SetValue(EnterCommandProperty, value);
    }

    public static ICommand? GetEnterCommand(MTreeItem obj)
    {
        return obj.GetValue(EnterCommandProperty);
    }

    public static readonly AttachedProperty<ICommand?> ExitCommandProperty =
        AvaloniaProperty.RegisterAttached<PointerCommands, MTreeItem, ICommand?>("ExitCommand");

    public static void SetExitCommand(MTreeItem obj, ICommand? value)
    {
        obj.SetValue(ExitCommandProperty, value);
    }

    public static ICommand? GetExitCommand(MTreeItem obj)
    {
        return obj.GetValue(ExitCommandProperty);
    }

    static PointerCommands()
    {
        EnterCommandProperty.Changed.AddClassHandler<MTreeItem>(HandleEnterCommandChanged);
        ExitCommandProperty.Changed.AddClassHandler<MTreeItem>(HandleExitCommandChanged);
    }

    static void HandleEnterCommandChanged(MTreeItem control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is ICommand)
            control.AddHandler(MTreeItem.HeaderPointerEnteredEvent, OnPointerEntered);
        else
            control.RemoveHandler(MTreeItem.HeaderPointerEnteredEvent, OnPointerEntered);
    }

    static void OnPointerEntered(object? sender, HeaderPointerEnteredArgs e)
    {
        if (sender is not MTreeItem control)
            return;

        var command = GetEnterCommand(control);
        if (command is null)
            return;

        if (command.CanExecute(null))
            command.Execute(null);
    }

    static void HandleExitCommandChanged(MTreeItem control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is ICommand)
            control.AddHandler(MTreeItem.HeaderPointerExitedEvent, OnPointerExited);
        else
            control.RemoveHandler(MTreeItem.HeaderPointerExitedEvent, OnPointerExited);
    }

    static void OnPointerExited(object? sender, HeaderPointerExitedArgs e)
    {
        if (sender is not MTreeItem control)
            return;

        var command = GetExitCommand(control);
        if (command is null)
            return;

        if (command.CanExecute(null))
            command.Execute(null);
    }
}
