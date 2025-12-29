using Manta.Reactive.Services;
using Manta.Reactive.Views;

namespace Manta.Tests.Reactive.Views;

/// <summary>
/// Comprehensive tests for ViewManager (Manta/Reactive/Views/ViewManager.cs)
/// Priority: HIGH - Manages view lifecycle and navigation
/// </summary>
public class ViewManagerTests : IDisposable
{
    private readonly ViewManager _viewManager;

    // Helper test ViewModels
    private class TestViewModel : ViewModel
    {
        public bool IsDisposed { get; private set; }

        protected override void OnDispose()
        {
            IsDisposed = true;
            base.OnDispose();
        }
    }

    private class AnotherTestViewModel : ViewModel
    {
        public string Data { get; set; } = string.Empty;
        public bool IsDisposed { get; private set; }

        protected override void OnDispose()
        {
            IsDisposed = true;
            base.OnDispose();
        }
    }

    private class ViewModelWithDependency : ViewModel
    {
        public object Dependency { get; }

        public ViewModelWithDependency(object dependency)
        {
            Dependency = dependency;
        }
    }

    private class NonDisposableViewModel : ViewModel
    {
        // Doesn't override OnDispose
    }

    public ViewManagerTests()
    {
        _viewManager = new ViewManager();
        ClearServices();
    }

    private void ClearServices()
    {
        var dicField = typeof(Service).GetField("_dic",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var dic = dicField?.GetValue(null) as System.Collections.IDictionary;
        dic?.Clear();
    }

    public void Dispose()
    {
        _viewManager.Dispose();
        ClearServices();
    }

    #region Current Property Tests

    [Fact]
    public void Current_Initially_ShouldBeNull()
    {
        // Assert
        _viewManager.Current.Should().BeNull();
    }

    [Fact]
    public void Current_AfterOpenView_ShouldBeSetToNewView()
    {
        // Arrange
        var viewModel = new TestViewModel();

        // Act
        _viewManager.OpenView(() => viewModel);

        // Assert
        _viewManager.Current.Should().BeSameAs(viewModel);
    }

    [Fact]
    public void Current_AfterClearView_ShouldBeNull()
    {
        // Arrange
        var viewModel = new TestViewModel();
        _viewManager.OpenView(() => viewModel);

        // Act
        _viewManager.ClearView();

        // Assert
        _viewManager.Current.Should().BeNull();
    }

    #endregion

    #region OpenView with Factory Tests

    [Fact]
    public void OpenView_WithFactory_ShouldSetCurrent()
    {
        // Arrange
        var viewModel = new TestViewModel();

        // Act
        _viewManager.OpenView(() => viewModel);

        // Assert
        _viewManager.Current.Should().BeSameAs(viewModel);
    }

    [Fact]
    public void OpenView_WithFactory_ShouldInvokeFactory()
    {
        // Arrange
        var factoryCalled = false;
        var viewModel = new TestViewModel();

        // Act
        _viewManager.OpenView(() =>
        {
            factoryCalled = true;
            return viewModel;
        });

        // Assert
        factoryCalled.Should().BeTrue();
    }

    [Fact]
    public void OpenView_WithFactoryTwice_ShouldDisposePreviousView()
    {
        // Arrange
        var firstView = new TestViewModel();
        var secondView = new AnotherTestViewModel();
        _viewManager.OpenView(() => firstView);

        // Act
        _viewManager.OpenView(() => secondView);

        // Assert
        firstView.IsDisposed.Should().BeTrue("previous view should be disposed");
        _viewManager.Current.Should().BeSameAs(secondView);
    }

    #endregion

    #region OpenView with Service Resolution Tests

    [Fact]
    public void OpenView_WithServiceResolution_ShouldCreateAndSetView()
    {
        // Act
        _viewManager.OpenView<TestViewModel>();

        // Assert
        _viewManager.Current.Should().NotBeNull();
        _viewManager.Current.Should().BeOfType<TestViewModel>();
    }

    [Fact]
    public void OpenView_WithServiceResolutionAndAction_ShouldInvokeAction()
    {
        // Arrange
        var actionInvoked = false;
        string dataSet = string.Empty;

        // Act
        _viewManager.OpenView<AnotherTestViewModel>(vm =>
        {
            actionInvoked = true;
            vm.Data = "Test Data";
            dataSet = vm.Data;
        });

        // Assert
        actionInvoked.Should().BeTrue();
        dataSet.Should().Be("Test Data");
        (_viewManager.Current as AnotherTestViewModel)?.Data.Should().Be("Test Data");
    }

    [Fact]
    public void OpenView_WithServiceResolutionAndDependency_ShouldResolveDependency()
    {
        // Arrange
        var dependency = new object();
        Service.Register(dependency);

        // Act
        _viewManager.OpenView<ViewModelWithDependency>();

        // Assert
        var current = _viewManager.Current as ViewModelWithDependency;
        current.Should().NotBeNull();
        current!.Dependency.Should().BeSameAs(dependency);
    }

    [Fact]
    public void OpenView_WithServiceResolutionTwice_ShouldDisposePreviousView()
    {
        // Arrange
        _viewManager.OpenView<TestViewModel>();
        var firstView = _viewManager.Current as TestViewModel;

        // Act
        _viewManager.OpenView<AnotherTestViewModel>();

        // Assert
        firstView.Should().NotBeNull();
        firstView!.IsDisposed.Should().BeTrue();
        _viewManager.Current.Should().BeOfType<AnotherTestViewModel>();
    }

    #endregion

    #region OpenView by Type Tests

    [Fact]
    public void OpenView_ByType_ShouldCreateCorrectType()
    {
        // Arrange
        var type = typeof(TestViewModel);

        // Act
        _viewManager.OpenView(type);

        // Assert
        _viewManager.Current.Should().NotBeNull();
        _viewManager.Current.Should().BeOfType<TestViewModel>();
    }

    [Fact]
    public void OpenView_ByTypeWithAction_ShouldInvokeAction()
    {
        // Arrange
        var type = typeof(AnotherTestViewModel);
        var actionInvoked = false;

        // Act
        _viewManager.OpenView<AnotherTestViewModel>(type, vm =>
        {
            actionInvoked = true;
            vm.Data = "Type Test";
        });

        // Assert
        actionInvoked.Should().BeTrue();
        (_viewManager.Current as AnotherTestViewModel)?.Data.Should().Be("Type Test");
    }

    [Fact]
    public void OpenView_ByTypeTwice_ShouldDisposePreviousView()
    {
        // Arrange
        var type1 = typeof(TestViewModel);
        var type2 = typeof(AnotherTestViewModel);
        _viewManager.OpenView(type1);
        var firstView = _viewManager.Current as TestViewModel;

        // Act
        _viewManager.OpenView(type2);

        // Assert
        firstView.Should().NotBeNull();
        firstView!.IsDisposed.Should().BeTrue();
        _viewManager.Current.Should().BeOfType<AnotherTestViewModel>();
    }

    #endregion

    #region ClearView Tests

    [Fact]
    public void ClearView_WhenCurrentIsNull_ShouldNotThrow()
    {
        // Act
        var act = () => _viewManager.ClearView();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ClearView_WhenCurrentExists_ShouldSetCurrentToNull()
    {
        // Arrange
        _viewManager.OpenView(() => new TestViewModel());

        // Act
        _viewManager.ClearView();

        // Assert
        _viewManager.Current.Should().BeNull();
    }

    [Fact]
    public void ClearView_WhenCurrentIsDisposable_ShouldDisposeIt()
    {
        // Arrange
        var viewModel = new TestViewModel();
        _viewManager.OpenView(() => viewModel);

        // Act
        _viewManager.ClearView();

        // Assert
        viewModel.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ClearView_WhenCurrentIsNotDisposable_ShouldNotThrow()
    {
        // Arrange
        _viewManager.OpenView(() => new NonDisposableViewModel());

        // Act
        var act = () => _viewManager.ClearView();

        // Assert
        act.Should().NotThrow();
        _viewManager.Current.Should().BeNull();
    }

    [Fact]
    public void ClearView_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        _viewManager.OpenView(() => new TestViewModel());
        _viewManager.ClearView();

        // Act
        var act = () => _viewManager.ClearView();

        // Assert
        act.Should().NotThrow();
        _viewManager.Current.Should().BeNull();
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_WhenCurrentExists_ShouldDisposeCurrentView()
    {
        // Arrange
        var viewModel = new TestViewModel();
        _viewManager.OpenView(() => viewModel);

        // Act
        _viewManager.Dispose();

        // Assert
        viewModel.IsDisposed.Should().BeTrue();
        _viewManager.Current.Should().BeNull();
    }

    [Fact]
    public void Dispose_WhenCurrentIsNull_ShouldNotThrow()
    {
        // Act
        var act = () => _viewManager.Dispose();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        _viewManager.OpenView(() => new TestViewModel());
        _viewManager.Dispose();

        // Act
        var act = () => _viewManager.Dispose();

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Property Change Notification Tests

    [Fact]
    public void Current_WhenChanged_ShouldRaisePropertyChanged()
    {
        // Arrange
        var eventRaised = false;
        _viewManager.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ViewManager.Current))
                eventRaised = true;
        };

        // Act
        _viewManager.OpenView(() => new TestViewModel());

        // Assert
        eventRaised.Should().BeTrue();
    }

    #endregion

    #region Memory Leak Tests

    [Fact]
    public void OpenView_RepeatedlyWithDisposal_ShouldNotLeakMemory()
    {
        // Arrange & Act - Open and dispose multiple views
        WeakReference weakRef1 = null!;
        WeakReference weakRef2 = null!;
        WeakReference weakRef3 = null!;

        // Scope 1
        {
            var vm1 = new TestViewModel();
            weakRef1 = new WeakReference(vm1);
            _viewManager.OpenView(() => vm1);
        }

        // Scope 2 - should dispose vm1
        {
            var vm2 = new TestViewModel();
            weakRef2 = new WeakReference(vm2);
            _viewManager.OpenView(() => vm2);
        }

        // Scope 3 - should dispose vm2
        {
            var vm3 = new TestViewModel();
            weakRef3 = new WeakReference(vm3);
            _viewManager.OpenView(() => vm3);
        }

        // Clear current
        _viewManager.ClearView();

        // Force garbage collection
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Assert - Previous views should be collected
        // Note: This is a weak test as GC behavior is non-deterministic
        // but it demonstrates the intent
        weakRef1.IsAlive.Should().BeFalse("first view should be garbage collected");
        weakRef2.IsAlive.Should().BeFalse("second view should be garbage collected");
        weakRef3.IsAlive.Should().BeFalse("third view should be garbage collected");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void FullWorkflow_OpenMultipleViewsWithDifferentMethods_ShouldWork()
    {
        // Test 1: Factory
        var vm1 = new TestViewModel();
        _viewManager.OpenView(() => vm1);
        _viewManager.Current.Should().BeSameAs(vm1);
        vm1.IsDisposed.Should().BeFalse();

        // Test 2: Service resolution
        _viewManager.OpenView<AnotherTestViewModel>();
        vm1.IsDisposed.Should().BeTrue("previous should be disposed");
        _viewManager.Current.Should().BeOfType<AnotherTestViewModel>();

        var vm2 = _viewManager.Current as AnotherTestViewModel;

        // Test 3: By type
        _viewManager.OpenView(typeof(TestViewModel));
        vm2!.IsDisposed.Should().BeTrue("previous should be disposed");
        _viewManager.Current.Should().BeOfType<TestViewModel>();

        // Test 4: Clear
        var vm3 = _viewManager.Current as TestViewModel;
        _viewManager.ClearView();
        vm3!.IsDisposed.Should().BeTrue("should be disposed on clear");
        _viewManager.Current.Should().BeNull();
    }

    #endregion
}
