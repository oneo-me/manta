using LightKit.Reactive.Services;
using LightKit.Demo.Reactive;

namespace LightKit.Demo.Services;

sealed class DemoService
{
    readonly List<DemoViewModel> _demos = [];

    public DemoService()
    {
        foreach (var type in GetType().Assembly.GetTypes().Where(x => typeof(DemoViewModel).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false }))
        {
            var demoViewModel = Service.CreateInstance<DemoViewModel>(type);
            _demos.Add(demoViewModel);
        }
    }

    public IReadOnlyCollection<DemoViewModel> GetDemos(DemoGroup group)
    {
        return _demos.Where(x => x.Group == group).ToArray();
    }
}
