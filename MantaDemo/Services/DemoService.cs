using MantaDemo.Demos.AutoDemos;
using MantaDemo.Demos.ControlDemos;
using MantaDemo.Demos.MultiLanguageDemos;
using MantaDemo.Demos.ReactiveDemos;
using MantaDemo.Demos.StyleDemos;
using MantaDemo.Reactive;

namespace MantaDemo.Services;

sealed class DemoService
{
    readonly List<DemoViewModel> _demos =
    [
        new IconView_Model(),

        new CommandView_Model(),
        new ContextView_Model(),
        new DialogView_Model(),
        new ViewTargetView_Model(),
        new LanguageView_Model(),

        new ColorView_Model(),
        new FontView_Model(),
        new RadiusView_Model(),
        new RingView_Model(),
        new ShadowView_Model(),
        new SpacingView_Model(),
        new ThicknessView_Model()
    ];

    public DemoService()
    {
        _demos.AddRange(AutoDemos.CreateDemos());

        _demos.Sort((a, b) =>
        {
            var subGroupComparison = a.SubGroup.CompareTo(b.SubGroup);
            if (subGroupComparison != 0)
                return subGroupComparison;

            var groupComparison = a.Group.CompareTo(b.Group);
            if (groupComparison != 0)
                return groupComparison;

            return string.Compare(a.Header, b.Header, StringComparison.Ordinal);
        });
    }

    public IEnumerable<DemoViewModel> GetDemos(DemoGroup group)
    {
        return _demos.Where(d => d.Group == group);
    }
}
