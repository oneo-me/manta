using Axe.Logging;
using AxeDemo.Interfaces;

namespace AxeDemo.Services;

sealed class DemoService
{
    readonly List<IDemoModel> _demos = [];

    public IReadOnlyCollection<IDemoModel> Demos => _demos;

    public DemoService()
    {
        foreach (var type in GetType().Assembly.GetTypes().Where(x => typeof(IDemoModel).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false }))
            Log.Warning("Adding demo: {Type}", type.FullName);
    }
}
