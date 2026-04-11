using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

static class AutoDemos
{
    public static IEnumerable<DemoViewModel> CreateDemos()
    {
        yield return new TextBoxView_Model();
        yield return new DropDownButtonView_Model();
        yield return new HyperlinkButtonView_Model();
        yield return new ToggleButtonView_Model();
        yield return new ProgressRingView_Model();
        yield return new SplitButtonView_Model();
        yield return new TabStripView_Model();
        yield return new MenuView_Model();
        yield return new ItemsView_Model();
        yield return new RadioButtonView_Model();
        yield return new CardView_Model();
        yield return new BackgroundView_Model();
        yield return new PathIconView_Model();
        yield return new AutoCompleteBoxView_Model();
        yield return new SeparatorView_Model();
        yield return new ComboBoxView_Model();
        yield return new ExpanderView_Model();
        yield return new CommandPanelView_Model();
        yield return new TitleBarView_Model();
        yield return new ViewHostView_Model();
        yield return new ToggleSwitchView_Model();
        yield return new CarouselView_Model();
        yield return new AspectRatioView_Model();
        yield return new ScrollViewerView_Model();
        yield return new ClipView_Model();
        yield return new ToggleSplitButtonView_Model();
        yield return new DialogTemplateView_Model();
        yield return new TabControlView_Model();
        yield return new ButtonView_Model();
        yield return new AlertView_Model();
        yield return new FlyoutView_Model();
        yield return new ButtonSpinnerView_Model();
        yield return new RingShapeView_Model();
        yield return new TreeView_Model();
        yield return new MaskedTextBoxView_Model();
        yield return new PixelGridView_Model();
        yield return new BreadcrumbView_Model();
        yield return new ImageView_Model();
        yield return new NumericUpDownView_Model();
        yield return new AccordionView_Model();
        yield return new SliderView_Model();
        yield return new RepeatButtonView_Model();
        yield return new TabTitleBarView_Model();
        yield return new BadgeView_Model();
        yield return new ListBoxView_Model();
        yield return new CheckBoxView_Model();
        yield return new ProgressBarView_Model();
        yield return new ToolTipView_Model();
    }
}
