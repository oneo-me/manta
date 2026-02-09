using System.Diagnostics;
using Manta.NodeEditor.Core;
using Manta.NodeEditor.Nodes;
using Manta.NodeEditor.Rendering;
using Manta.NodeEditor.TypeSystem;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<NodeEditorView>]
sealed class NodeEditorView_Model : DemoViewModel
{
    public static NodeEditorView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(NodeCanvas);

    public NodeGraph Graph { get; } = new();

    public CommandWarper AddNumberNodeCommand { get; }
    public CommandWarper AddAddNodeCommand { get; }
    public CommandWarper AddSubtractNodeCommand { get; }
    public CommandWarper AddMultiplyNodeCommand { get; }
    public CommandWarper AddTextNodeCommand { get; }
    public CommandWarper AddConcatNodeCommand { get; }
    public CommandWarper ExecuteCommand { get; }

    double _nextX = 50;
    double _nextY = 50;

    public NodeEditorView_Model()
    {
        AddNumberNodeCommand = new(() => AddNode<NumberNode>());
        AddAddNodeCommand = new(() => AddNode<AddNode>());
        AddSubtractNodeCommand = new(() => AddNode<SubtractNode>());
        AddMultiplyNodeCommand = new(() => AddNode<MultiplyNode>());
        AddTextNodeCommand = new(() => AddNode<TextNode>());
        AddConcatNodeCommand = new(() => AddNode<ConcatNode>());
        ExecuteCommand = new(ExecuteGraph);

        Graph.TypeCompatibility = TypeCompatibility.CreateDefault();

        InitDemo();
    }

    void AddNode<T>() where T : NodeBase, new()
    {
        Graph.Add<T>(_nextX, _nextY);
        AdvancePosition();
    }

    void AdvancePosition()
    {
        _nextY += 120;
        if (_nextY > 500)
        {
            _nextY = 50;
            _nextX += 250;
        }
    }

    void InitDemo()
    {
        var num1 = Graph.Add<NumberNode>(50, 50);
        var num2 = Graph.Add<NumberNode>(50, 220);
        var add = Graph.Add<AddNode>(350, 130);

        num1.Result.Value = 10;
        num2.Result.Value = 20;

        Graph.Connect(num1.Result, add.A);
        Graph.Connect(num2.Result, add.B);

        _nextX = 600;
        _nextY = 50;
    }

    async Task ExecuteGraph()
    {
        try
        {
            // Simple topological execution: execute nodes in order
            var executed = new HashSet<Guid>();
            var sw = Stopwatch.StartNew();

            bool progress;
            do
            {
                progress = false;
                foreach (var node in Graph.Nodes)
                {
                    if (executed.Contains(node.Id)) continue;

                    // Check all inputs are satisfied
                    var ready = node.Inputs.All(input =>
                    {
                        var conn = Graph.Connections.FirstOrDefault(c => c.Target == input);
                        return conn is null || executed.Contains(conn.Source.Node!.Id);
                    });

                    if (!ready) continue;

                    // Propagate values from connections
                    foreach (var input in node.Inputs)
                    {
                        var conn = Graph.Connections.FirstOrDefault(c => c.Target == input);
                        if (conn is not null)
                            input.BoxedValue = conn.Source.BoxedValue;
                    }

                    await node.ExecuteAsync();
                    executed.Add(node.Id);
                    progress = true;
                }
            } while (progress && executed.Count < Graph.Nodes.Count);

            sw.Stop();
            Debug.WriteLine($"Graph executed in {sw.ElapsedMilliseconds}ms ({executed.Count}/{Graph.Nodes.Count} nodes)");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Execution failed: {ex}");
        }
    }
}
