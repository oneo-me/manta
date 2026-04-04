using MantaDemo.Reactive;

namespace MantaDemo.Services;

sealed class DemoService
{
    public IEnumerable<DemoViewModel> GetDemos(DemoGroup group)
    {
        yield break;
    }
}
