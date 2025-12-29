using Manta.Reactive.Services;

namespace Manta.Tests.Reactive.Services;

/// <summary>
/// Comprehensive tests for the Service container (Manta/Reactive/Services/Service.cs)
/// Priority: HIGH - Core dependency injection used throughout the application
/// </summary>
public class ServiceTests
{
    // Helper test classes
    private interface ITestService { }
    private class TestServiceImpl : ITestService { }

    private interface IAnotherService { }
    private class AnotherServiceImpl : IAnotherService { }

    private class ServiceWithDependency
    {
        public ITestService TestService { get; }
        public ServiceWithDependency(ITestService testService)
        {
            TestService = testService;
        }
    }

    private class ServiceWithMultipleDependencies
    {
        public ITestService TestService { get; }
        public IAnotherService AnotherService { get; }

        public ServiceWithMultipleDependencies(ITestService testService, IAnotherService anotherService)
        {
            TestService = testService;
            AnotherService = anotherService;
        }
    }

    private class ServiceWithMultipleConstructors
    {
        public int ConstructorUsed { get; }

        // This should be selected (fewest parameters)
        public ServiceWithMultipleConstructors()
        {
            ConstructorUsed = 0;
        }

        public ServiceWithMultipleConstructors(ITestService service)
        {
            ConstructorUsed = 1;
        }

        public ServiceWithMultipleConstructors(ITestService service1, IAnotherService service2)
        {
            ConstructorUsed = 2;
        }
    }

    /// <summary>
    /// Clear the service dictionary between tests to ensure isolation
    /// Note: This uses reflection since _dic is private
    /// </summary>
    private void ClearServices()
    {
        var dicField = typeof(Service).GetField("_dic",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var dic = dicField?.GetValue(null) as System.Collections.IDictionary;
        dic?.Clear();
    }

    public ServiceTests()
    {
        ClearServices();
    }

    #region Registration Tests

    [Fact]
    public void Register_SingleType_ShouldReturnSameInstance()
    {
        // Arrange
        var expected = new TestServiceImpl();

        // Act
        var result = Service.Register(expected);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void Register_SingleType_ShouldBeRetrievable()
    {
        // Arrange
        var expected = new TestServiceImpl();
        Service.Register(expected);

        // Act
        var result = Service.GetService<TestServiceImpl>();

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void Register_InterfaceAndImplementation_ShouldReturnSameInstance()
    {
        // Arrange
        var expected = new TestServiceImpl();

        // Act
        var result = Service.Register<ITestService, TestServiceImpl>(expected);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void Register_InterfaceAndImplementation_BothShouldBeRetrievable()
    {
        // Arrange
        var expected = new TestServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(expected);

        // Act
        var resultByInterface = Service.GetService<ITestService>();
        var resultByImpl = Service.GetService<TestServiceImpl>();

        // Assert
        resultByInterface.Should().BeSameAs(expected);
        resultByImpl.Should().BeSameAs(expected);
        resultByInterface.Should().BeSameAs(resultByImpl);
    }

    [Fact]
    public void Register_DuplicateServiceType_ShouldThrowServiceException()
    {
        // Arrange
        var first = new TestServiceImpl();
        var second = new TestServiceImpl();
        Service.Register(first);

        // Act
        var act = () => Service.Register(second);

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*already been registered*");
    }

    [Fact]
    public void Register_DuplicateImplementationType_ShouldThrowServiceException()
    {
        // Arrange
        var first = new TestServiceImpl();
        var second = new TestServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(first);

        // Act - Register same impl type with different interface
        var act = () => Service.Register<IAnotherService, TestServiceImpl>(second as dynamic);

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*already been registered*");
    }

    [Fact]
    public void Register_InvalidTypeHierarchy_ShouldThrowServiceException()
    {
        // Arrange
        var impl = new TestServiceImpl();

        // Act - Try to register TestServiceImpl as IAnotherService (invalid)
        var act = () => Service.Register<IAnotherService, TestServiceImpl>(impl);

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*is not a subclass of*");
    }

    #endregion

    #region GetService Tests

    [Fact]
    public void GetService_UnregisteredService_ShouldThrowServiceException()
    {
        // Act
        var act = () => Service.GetService<ITestService>();

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*not registered*");
    }

    [Fact]
    public void GetService_RegisteredService_ShouldReturnCorrectInstance()
    {
        // Arrange
        var expected = new TestServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(expected);

        // Act
        var result = Service.GetService<ITestService>();

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public void GetService_MultipleRegistrations_ShouldReturnCorrectInstances()
    {
        // Arrange
        var service1 = new TestServiceImpl();
        var service2 = new AnotherServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(service1);
        Service.Register<IAnotherService, AnotherServiceImpl>(service2);

        // Act
        var result1 = Service.GetService<ITestService>();
        var result2 = Service.GetService<IAnotherService>();

        // Assert
        result1.Should().BeSameAs(service1);
        result2.Should().BeSameAs(service2);
    }

    #endregion

    #region CreateInstance Tests

    [Fact]
    public void CreateInstance_ParameterlessConstructor_ShouldCreateInstance()
    {
        // Act
        var result = Service.CreateInstance<TestServiceImpl>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TestServiceImpl>();
    }

    [Fact]
    public void CreateInstance_WithDependency_ShouldResolveAndCreate()
    {
        // Arrange
        var dependency = new TestServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(dependency);

        // Act
        var result = Service.CreateInstance<ServiceWithDependency>();

        // Assert
        result.Should().NotBeNull();
        result.TestService.Should().BeSameAs(dependency);
    }

    [Fact]
    public void CreateInstance_WithMultipleDependencies_ShouldResolveAll()
    {
        // Arrange
        var dependency1 = new TestServiceImpl();
        var dependency2 = new AnotherServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(dependency1);
        Service.Register<IAnotherService, AnotherServiceImpl>(dependency2);

        // Act
        var result = Service.CreateInstance<ServiceWithMultipleDependencies>();

        // Assert
        result.Should().NotBeNull();
        result.TestService.Should().BeSameAs(dependency1);
        result.AnotherService.Should().BeSameAs(dependency2);
    }

    [Fact]
    public void CreateInstance_WithMissingDependency_ShouldThrowServiceException()
    {
        // Act - ServiceWithDependency requires ITestService which is not registered
        var act = () => Service.CreateInstance<ServiceWithDependency>();

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*not registered*");
    }

    [Fact]
    public void CreateInstance_MultipleConstructors_ShouldUseOneWithFewestParameters()
    {
        // Act
        var result = Service.CreateInstance<ServiceWithMultipleConstructors>();

        // Assert
        result.Should().NotBeNull();
        result.ConstructorUsed.Should().Be(0, "should use parameterless constructor");
    }

    [Fact]
    public void CreateInstance_ByType_ShouldCreateCorrectType()
    {
        // Arrange
        var type = typeof(TestServiceImpl);

        // Act
        var result = Service.CreateInstance<ITestService>(type);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TestServiceImpl>();
    }

    [Fact]
    public void CreateInstance_InvalidTypeHierarchy_ShouldThrowServiceException()
    {
        // Arrange
        var type = typeof(TestServiceImpl);

        // Act - Try to create TestServiceImpl as IAnotherService (invalid)
        var act = () => Service.CreateInstance<IAnotherService>(type);

        // Assert
        act.Should().Throw<ServiceException>()
            .WithMessage("*is not a subclass of*");
    }

    [Fact]
    public void CreateInstance_NoAvailableConstructor_ShouldThrowServiceException()
    {
        // Arrange - Abstract class has no constructors we can use
        var type = typeof(AbstractService);

        // Act
        var act = () => Service.CreateInstance<AbstractService>(type);

        // Assert
        // This will throw because Activator.CreateInstance fails on abstract classes
        act.Should().Throw<Exception>();
    }

    private abstract class AbstractService { }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public void Register_ConcurrentRegistrations_ShouldHandleSafely()
    {
        // Arrange
        var tasks = new List<Task>();
        var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

        // Act - Try to register different services concurrently
        for (int i = 0; i < 10; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    // Each task registers a unique service
                    var service = new TestServiceImpl();
                    Service.Register($"Service{index}", service);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert - No exceptions should occur with different services
        exceptions.Should().BeEmpty();
    }

    [Fact]
    public void GetService_ConcurrentRetrievals_ShouldReturnSameInstance()
    {
        // Arrange
        var expected = new TestServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(expected);
        var results = new System.Collections.Concurrent.ConcurrentBag<ITestService>();
        var tasks = new List<Task>();

        // Act - Multiple threads retrieving the same service
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var service = Service.GetService<ITestService>();
                results.Add(service);
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert - All retrievals should return the same instance
        results.Should().HaveCount(100);
        results.Should().AllBe(expected);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void FullWorkflow_RegisterMultipleServicesAndCreateComplexInstance_ShouldWork()
    {
        // Arrange - Set up a dependency chain
        var testService = new TestServiceImpl();
        var anotherService = new AnotherServiceImpl();
        Service.Register<ITestService, TestServiceImpl>(testService);
        Service.Register<IAnotherService, AnotherServiceImpl>(anotherService);

        // Act - Create an instance that depends on both
        var complex = Service.CreateInstance<ServiceWithMultipleDependencies>();

        // Also verify we can retrieve the registered services
        var retrieved1 = Service.GetService<ITestService>();
        var retrieved2 = Service.GetService<IAnotherService>();

        // Assert
        complex.Should().NotBeNull();
        complex.TestService.Should().BeSameAs(testService);
        complex.AnotherService.Should().BeSameAs(anotherService);
        retrieved1.Should().BeSameAs(testService);
        retrieved2.Should().BeSameAs(anotherService);
    }

    #endregion
}
