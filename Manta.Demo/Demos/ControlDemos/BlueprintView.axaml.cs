using Manta.Controls;

namespace Manta.Demo.Demos.ControlDemos;

sealed partial class BlueprintView : MView
{
    public BlueprintView()
    {
        InitializeComponent();

        // Wire up the blueprint control to the view model
        if (DataContext is BlueprintView_Model viewModel)
        {
            viewModel.SetBlueprint(Blueprint);
        }

        DataContextChanged += (_, _) =>
        {
            if (DataContext is BlueprintView_Model vm)
            {
                vm.SetBlueprint(Blueprint);
            }
        };
    }
}
