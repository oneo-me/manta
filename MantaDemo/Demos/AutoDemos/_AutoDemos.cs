using MantaDemo.Reactive;

namespace MantaDemo.Demos.AutoDemos;

static class AutoDemos
{
    public static IEnumerable<DemoViewModel> CreateDemos()
    {
        yield return new MAspectRatioView_Model();
        yield return new MImageView_Model();
        yield return new MToggleSwitchView_Model();
        yield return new MScrollViewerView_Model();
        yield return new MAccordionView_Model();
        yield return new MListBoxView_Model();
        yield return new MButtonView_Model();
        yield return new MPixelGridView_Model();
        yield return new MBackgroundView_Model();
        yield return new MAutoCompleteBoxView_Model();
        yield return new MTextBoxView_Model();
        yield return new MCarouselView_Model();
        yield return new MAlertView_Model();
        yield return new MTitleBarView_Model();
        yield return new MRadioButtonView_Model();
        yield return new MToggleButtonView_Model();
        yield return new MCardView_Model();
        yield return new MItemsView_Model();
        yield return new MFlyoutView_Model();
        yield return new MProgressRingView_Model();
        yield return new MSeparatorView_Model();
        yield return new MExpanderView_Model();
        yield return new MRingShapeView_Model();
        yield return new MSliderView_Model();
        yield return new MProgressBarView_Model();
        yield return new MMaskedTextBoxView_Model();
        yield return new MBreadcrumbView_Model();
        yield return new MButtonSpinnerView_Model();
        yield return new MBadgeView_Model();
        yield return new MClipView_Model();
        yield return new MPathIconView_Model();
        yield return new MTreeView_Model();
        yield return new MHyperlinkButtonView_Model();
        yield return new MCheckBoxView_Model();
        yield return new MSplitButtonView_Model();
        yield return new MDropDownButtonView_Model();
        yield return new MDialogTemplateView_Model();
        yield return new MTabTitleBarView_Model();
        yield return new MNumericUpDownView_Model();
        yield return new MViewHostView_Model();
        yield return new MRepeatButtonView_Model();
        yield return new MMenuView_Model();
        yield return new MTabStripView_Model();
        yield return new MToggleSplitButtonView_Model();
        yield return new MCommandPanelView_Model();
        yield return new MComboBoxView_Model();
        yield return new MTabControlView_Model();
        yield return new ToolTipView_Model();
    }
}
