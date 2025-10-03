using Axe.Reactive.Services;
using AxeDemo.Reactive;

namespace AxeDemo.Services;

sealed class DemoService
{
    readonly List<DemoViewModel> _demos = [];

    public DemoService()
    {
        foreach (var type in GetType().Assembly.GetTypes().Where(x => typeof(DemoViewModel).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false }))
            for (var i = 0; i < 10; i++)
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
