using Avalonia.Controls;
using Manta.Controls;
using Manta.NodeEditor.Core;

namespace Manta.NodeEditor.Nodes;

/// <summary>
/// Outputs a constant number value. The user can edit the value via the inline editor.
/// </summary>
public class NumberNode : NodeBase
{
    public override string Title => "Number";
    public override string Category => "Math";

    public OutputPort<double> Result { get; } = new("Result");

    public override Control BuildContent()
    {
        var nud = new MNumericUpDown
        {
            Value = (decimal)Result.Value,
            Width = 100,
            FontSize = 11,
            FormatString = "F2",
            Minimum = decimal.MinValue,
            Maximum = decimal.MaxValue
        };

        nud.ValueChanged += (_, e) => Result.Value = (double)(e.NewValue ?? 0m);
        return nud;
    }
}

/// <summary>
/// Adds two numbers.
/// </summary>
public class AddNode : NodeBase
{
    public override string Title => "Add";
    public override string Category => "Math";

    public InputPort<double> A { get; } = new("A");
    public InputPort<double> B { get; } = new("B");
    public OutputPort<double> Result { get; } = new("Result");

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result.Value = A.Value + B.Value;
        return Task.CompletedTask;
    }
}

/// <summary>
/// Subtracts B from A.
/// </summary>
public class SubtractNode : NodeBase
{
    public override string Title => "Subtract";
    public override string Category => "Math";

    public InputPort<double> A { get; } = new("A");
    public InputPort<double> B { get; } = new("B");
    public OutputPort<double> Result { get; } = new("Result");

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result.Value = A.Value - B.Value;
        return Task.CompletedTask;
    }
}

/// <summary>
/// Multiplies two numbers.
/// </summary>
public class MultiplyNode : NodeBase
{
    public override string Title => "Multiply";
    public override string Category => "Math";

    public InputPort<double> A { get; } = new("A");
    public InputPort<double> B { get; } = new("B");
    public OutputPort<double> Result { get; } = new("Result");

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result.Value = A.Value * B.Value;
        return Task.CompletedTask;
    }
}
