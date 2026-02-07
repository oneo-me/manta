using Avalonia.Controls;
using Manta.NodeEditor.Core;

namespace Manta.NodeEditor.Nodes;

/// <summary>
/// Outputs a constant text value. The user can edit the value via the inline editor.
/// </summary>
public class TextNode : NodeBase
{
    public override string Title => "Text";
    public override string Category => "String";

    public OutputPort<string> Result { get; } = new("Text");

    public override Control? BuildContent()
    {
        var tb = new TextBox
        {
            Text = Result.Value ?? "",
            Width = 120,
            FontSize = 11
        };

        tb.TextChanged += (_, _) => Result.Value = tb.Text ?? "";
        return tb;
    }
}

/// <summary>
/// Concatenates two strings.
/// </summary>
public class ConcatNode : NodeBase
{
    public override string Title => "Concat";
    public override string Category => "String";

    public InputPort<string> A { get; } = new("A");
    public InputPort<string> B { get; } = new("B");
    public OutputPort<string> Result { get; } = new("Result");

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Result.Value = (A.Value ?? "") + (B.Value ?? "");
        return Task.CompletedTask;
    }
}
