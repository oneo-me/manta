using Avalonia.Media;
using Manta.MindMap.Controls;
using Manta.MindMap.Models;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

[View<MindMapView>]
sealed class MindMapView_Model : DemoViewModel
{
    public static MindMapView_Model Design { get; } = new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MMindMap);

    public MindMapGraph MindMap { get; } = new();

    public CommandWarper AddChildCommand => GetCommand(AddChild);
    public CommandWarper DeleteCommand => GetCommand(DeleteSelected);
    public CommandWarper ResetViewCommand => GetCommand(ResetView);

    public MindMapView_Model()
    {
        InitializeRootNode();
    }

    void InitializeRootNode()
    {
        var root = new MindMapNode
        {
            Text = "Central Topic",
            IsRoot = true,
            Width = 160,
            Height = 50,
            Background = Brushes.DodgerBlue,
            Foreground = Brushes.White
        };

        MindMap.SetRoot(root);

        var branch1 = MindMap.AddNode("Branch 1", root);
        MindMap.AddNode("Sub 1.1", branch1);
        MindMap.AddNode("Sub 1.2", branch1);

        var branch2 = MindMap.AddNode("Branch 2", root);
        MindMap.AddNode("Sub 2.1", branch2);
        MindMap.AddNode("Sub 2.2", branch2);
        MindMap.AddNode("Sub 2.3", branch2);

        var branch3 = MindMap.AddNode("Branch 3", root);
        MindMap.AddNode("Sub 3.1", branch3);

        MindMap.RefreshLayout();
    }

    void AddChild()
    {
        var selectedNode = MindMap.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selectedNode != null)
        {
            var childCount = selectedNode.Children.Count;
            MindMap.AddNode($"Node {childCount + 1}", selectedNode);
            MindMap.RefreshLayout();
        }
        else if (MindMap.RootNode != null)
        {
            MindMap.AddNode("New Branch", MindMap.RootNode);
            MindMap.RefreshLayout();
        }
    }

    void DeleteSelected()
    {
        var selectedNode = MindMap.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selectedNode is { IsRoot: false })
        {
            MindMap.RemoveNode(selectedNode);
            MindMap.RefreshLayout();
        }
    }

    void ResetView()
    {
        MindMap.RefreshLayout();
    }
}
