using Manta.Controls;

namespace MantaDemo.Demos.ControlDemos;

sealed partial class NodeEditorView : MView
{
    public NodeEditorView()
    {
        InitializeComponent();

        // Wire up the blueprint control to the view model
        if (DataContext is NodeEditorView_Model viewModel)
        {
            viewModel.SetNodeEditor(NodeEditor);
        }

        DataContextChanged += (_, _) =>
        {
            if (DataContext is NodeEditorView_Model vm)
            {
                vm.SetNodeEditor(NodeEditor);
            }
        };
    }
}
