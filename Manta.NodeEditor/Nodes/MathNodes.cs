using Manta.NodeEditor.Attributes;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Nodes;

[Node(DisplayName = "Add", Category = "Math")]
public class AddNode : NodeBase
{
    [Input(DisplayName = "A", Order = 0)]
    public double A { get; set; }

    [Input(DisplayName = "B", Order = 1)]
    public double B { get; set; }

    [Output(DisplayName = "Result", Order = 0)]
    public double Result { get; set; }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = A + B;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Subtract", Category = "Math")]
public class SubtractNode : NodeBase
{
    [Input(DisplayName = "A", Order = 0)]
    public double A { get; set; }

    [Input(DisplayName = "B", Order = 1)]
    public double B { get; set; }

    [Output(DisplayName = "Result", Order = 0)]
    public double Result { get; set; }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = A - B;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Multiply", Category = "Math")]
public class MultiplyNode : NodeBase
{
    [Input(DisplayName = "A", Order = 0)]
    public double A { get; set; }

    [Input(DisplayName = "B", Order = 1)]
    public double B { get; set; }

    [Output(DisplayName = "Result", Order = 0)]
    public double Result { get; set; }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = A * B;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Divide", Category = "Math")]
public class DivideNode : NodeBase
{
    [Input(DisplayName = "A", Order = 0)]
    public double A { get; set; }

    [Input(DisplayName = "B", Order = 1)]
    public double B { get; set; }

    [Output(DisplayName = "Result", Order = 0)]
    public double Result { get; set; }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = B != 0 ? A / B : 0;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Number", Category = "Math")]
public class NumberNode : NodeBase
{
    public double Value { get; set; }

    [Output(DisplayName = "Value", Order = 0)]
    public double OutputValue { get; set; }

    public override string DisplayName => $"Number: {Value}";

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        OutputValue = Value;
        return Task.CompletedTask;
    }
}
