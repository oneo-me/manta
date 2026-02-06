using System.Diagnostics;
using Manta.NodeEditor;
using Manta.NodeEditor.Controls;
using Manta.NodeEditor.Models;
using Manta.NodeEditor.Nodes;
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
    public override string Header => nameof(MNodeEditor);

    public Graph Graph { get; }
    public NodeRegistry Registry { get; }

    public CommandWarper AddNumberNodeCommand { get; }
    public CommandWarper AddAddNodeCommand { get; }
    public CommandWarper AddMultiplyNodeCommand { get; }
    public CommandWarper ExecuteCommand { get; }
    public CommandWarper ResetViewCommand { get; }

    MNodeEditor? _blueprint;
    double _nextNodeX = 50;
    double _nextNodeY = 50;

    public void SetNodeEditor(MNodeEditor? blueprint)
    {
        _blueprint = blueprint;
    }

    public NodeEditorView_Model()
    {
        Registry = new();
        Registry.ScanAssembly(typeof(AddNode).Assembly);

        Graph = new();

        AddNumberNodeCommand = new(AddNumberNode);
        AddAddNodeCommand = new(AddAddNode);
        AddMultiplyNodeCommand = new(AddMultiplyNode);
        ExecuteCommand = new(Execute);
        ResetViewCommand = new(ResetView);

        InitializeDemoGraph();
    }

    void InitializeDemoGraph()
    {
        var numberTypeInfo = Registry.GetNodeTypeInfo(typeof(NumberNode));
        var addTypeInfo = Registry.GetNodeTypeInfo(typeof(AddNode));

        if (numberTypeInfo == null || addTypeInfo == null) return;

        var num1 = Graph.AddNode(numberTypeInfo, new(50, 50));
        var num2 = Graph.AddNode(numberTypeInfo, new(50, 200));
        var addNode = Graph.AddNode(addTypeInfo, new(300, 125));

        if (num1.Instance is NumberNode n1) n1.Value = 10;
        if (num2.Instance is NumberNode n2) n2.Value = 20;

        if (num1.OutputPorts.Count > 0 && addNode.InputPorts.Count > 0)
            Graph.AddConnection(num1.OutputPorts[0], addNode.InputPorts[0]);

        if (num2.OutputPorts.Count > 0 && addNode.InputPorts.Count > 1)
            Graph.AddConnection(num2.OutputPorts[0], addNode.InputPorts[1]);

        _nextNodeX = 500;
        _nextNodeY = 50;
    }

    void AddNumberNode()
    {
        var typeInfo = Registry.GetNodeTypeInfo(typeof(NumberNode));
        if (typeInfo != null)
        {
            Graph.AddNode(typeInfo, new(_nextNodeX, _nextNodeY));
            _blueprint?.Refresh();
            AdvanceNodePosition();
        }
    }

    void AddAddNode()
    {
        var typeInfo = Registry.GetNodeTypeInfo(typeof(AddNode));
        if (typeInfo != null)
        {
            Graph.AddNode(typeInfo, new(_nextNodeX, _nextNodeY));
            _blueprint?.Refresh();
            AdvanceNodePosition();
        }
    }

    void AddMultiplyNode()
    {
        var typeInfo = Registry.GetNodeTypeInfo(typeof(MultiplyNode));
        if (typeInfo != null)
        {
            Graph.AddNode(typeInfo, new(_nextNodeX, _nextNodeY));
            _blueprint?.Refresh();
            AdvanceNodePosition();
        }
    }

    void AdvanceNodePosition()
    {
        _nextNodeY += 100;
        if (_nextNodeY > 400)
        {
            _nextNodeY = 50;
            _nextNodeX += 200;
        }
    }

    async Task Execute()
    {
        if (_blueprint == null) return;
        try
        {
            await _blueprint.ExecuteAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Execute failed: {ex}");
        }
    }

    void ResetView()
    {
        _blueprint?.ResetView();
    }
}
