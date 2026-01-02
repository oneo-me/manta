using Manta.Blueprint.Attributes;
using Manta.Blueprint.Models;

namespace Manta.Blueprint.Nodes;

[Node(DisplayName = "Concat", Category = "String")]
public class ConcatNode : NodeBase
{
    [Input(DisplayName = "A", Order = 0)]
    public string A { get; set; } = "";

    [Input(DisplayName = "B", Order = 1)]
    public string B { get; set; } = "";

    [Output(DisplayName = "Result", Order = 0)]
    public string Result { get; set; } = "";

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = A + B;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Text", Category = "String")]
public class TextNode : NodeBase
{
    public string Value { get; set; } = "";

    [Output(DisplayName = "Text", Order = 0)]
    public string OutputValue { get; set; } = "";

    public override string DisplayName => $"Text: \"{(Value.Length > 10 ? Value[..10] + "..." : Value)}\"";

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        OutputValue = Value;
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "To String", Category = "String")]
public class ToStringNode : NodeBase
{
    [Input(DisplayName = "Value", Order = 0)]
    public object? Value { get; set; }

    [Output(DisplayName = "Text", Order = 0)]
    public string Result { get; set; } = "";

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result = Value?.ToString() ?? "";
        return Task.CompletedTask;
    }
}

[Node(DisplayName = "Print", Category = "Debug")]
public class PrintNode : NodeBase
{
    [Input(DisplayName = "Message", Order = 0)]
    public string Message { get; set; } = "";

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Blueprint] {Message}");
        return Task.CompletedTask;
    }
}
