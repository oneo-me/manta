using Manta.Reactive.Regions;
using Manta.Reactive.Views;
using System.Collections.Specialized;

namespace Manta.Tests.Reactive.Regions;

/// <summary>
/// Comprehensive tests for RegionManager (Manta/Reactive/Regions/RegionManager`.cs)
/// Priority: HIGH - Complex threading logic with Lock, tab/region management
/// </summary>
public class RegionManagerTests : IDisposable
{
    // Helper test classes
    private class TestRegion : RegionModel
    {
        public string Data { get; set; } = string.Empty;
        public bool IsDisposed { get; private set; }

        protected override void OnDispose()
        {
            IsDisposed = true;
            base.OnDispose();
        }
    }

    private class AnotherRegion : RegionModel
    {
        public int Value { get; set; }
    }

    private class TargetableRegion : RegionModel, IRegionTarget<string>
    {
        public string TargetId { get; }

        public TargetableRegion(string targetId)
        {
            TargetId = targetId;
        }

        public bool IsTarget(string context)
        {
            return TargetId == context;
        }
    }

    private class TargetableRegionWithNumber : RegionModel, IRegionTarget<int>
    {
        public int TargetNumber { get; }

        public TargetableRegionWithNumber(int targetNumber)
        {
            TargetNumber = targetNumber;
        }

        public bool IsTarget(int context)
        {
            return TargetNumber == context;
        }
    }

    private readonly RegionManager<TestRegion> _manager;

    public RegionManagerTests()
    {
        _manager = new RegionManager<TestRegion>();
    }

    public void Dispose()
    {
        _manager.Dispose();
    }

    #region Initialization Tests

    [Fact]
    public void Constructor_WithoutInitialItems_ShouldHaveEmptyItems()
    {
        // Arrange & Act
        var manager = new RegionManager<TestRegion>();

        // Assert
        manager.Items.Should().BeEmpty();
        manager.SelectedItem.Should().BeNull();

        manager.Dispose();
    }

    [Fact]
    public void Constructor_WithInitialItems_ShouldPopulateItems()
    {
        // Arrange
        var region1 = new TestRegion { Data = "Region1" };
        var region2 = new TestRegion { Data = "Region2" };

        // Act
        var manager = new RegionManager<TestRegion>(region1, region2);

        // Assert
        manager.Items.Should().HaveCount(2);
        manager.Items.Should().Contain(region1);
        manager.Items.Should().Contain(region2);

        manager.Dispose();
    }

    [Fact]
    public void Items_ShouldBeReadOnly()
    {
        // Assert
        _manager.Items.Should().BeAssignableTo<System.Collections.ObjectModel.ReadOnlyObservableCollection<TestRegion>>();
    }

    #endregion

    #region Open Tests - Basic

    [Fact]
    public void Open_NewRegion_ShouldAddToItems()
    {
        // Act
        _manager.Open(() => new TestRegion { Data = "Test" });

        // Assert
        _manager.Items.Should().HaveCount(1);
        _manager.Items[0].Data.Should().Be("Test");
    }

    [Fact]
    public void Open_NewRegion_ShouldSetAsSelected()
    {
        // Act
        _manager.Open(() => new TestRegion { Data = "Test" });

        // Assert
        _manager.SelectedItem.Should().NotBeNull();
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[0]);
    }

    [Fact]
    public void Open_MultipleRegions_ShouldAddAll()
    {
        // Act
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });

        // Assert
        _manager.Items.Should().HaveCount(3);
        _manager.Items[0].Data.Should().Be("Region1");
        _manager.Items[1].Data.Should().Be("Region2");
        _manager.Items[2].Data.Should().Be("Region3");
    }

    [Fact]
    public void Open_NewRegion_ShouldSetCloseFuncOnRegion()
    {
        // Arrange
        TestRegion? createdRegion = null;

        // Act
        _manager.Open(() =>
        {
            var region = new TestRegion { Data = "Test" };
            createdRegion = region;
            return region;
        });

        // Assert
        createdRegion.Should().NotBeNull();
        createdRegion!.CloseFunc.Should().NotBeNull();
    }

    [Fact]
    public void Open_WithIsActiveFalse_ShouldNotSetAsSelected()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "First" });

        // Act
        _manager.Open(() => new TestRegion { Data = "Second" }, isActive: _ => false);

        // Assert
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[0]);
        _manager.SelectedItem!.Data.Should().Be("First");
    }

    [Fact]
    public void Open_WithIsActiveTrue_ShouldSetAsSelected()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "First" });

        // Act
        _manager.Open(() => new TestRegion { Data = "Second" }, isActive: _ => true);

        // Assert
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[1]);
        _manager.SelectedItem!.Data.Should().Be("Second");
    }

    #endregion

    #region Open Tests - Context and Targeting

    [Fact]
    public void Open_WithContext_NewRegion_ShouldCreate()
    {
        // Arrange
        var manager = new RegionManager<TargetableRegion>();

        // Act
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));

        // Assert
        manager.Items.Should().HaveCount(1);
        manager.Items[0].TargetId.Should().Be("context1");

        manager.Dispose();
    }

    [Fact]
    public void Open_WithContext_ExistingTarget_ShouldNotCreateNew()
    {
        // Arrange
        var manager = new RegionManager<TargetableRegion>();
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));

        // Act
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));

        // Assert
        manager.Items.Should().HaveCount(1, "should reuse existing region");
    }

    [Fact]
    public void Open_WithContext_ExistingTarget_ShouldActivateIt()
    {
        // Arrange
        var manager = new RegionManager<TargetableRegion>();
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));
        manager.Open<TargetableRegion, string>("context2", ctx => new TargetableRegion(ctx));
        manager.SelectedItem = manager.Items[1]; // Select context2

        // Act
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));

        // Assert
        manager.SelectedItem.Should().BeSameAs(manager.Items[0], "should activate existing context1");

        manager.Dispose();
    }

    [Fact]
    public void Open_WithContext_ExistingTargetButIsActiveFalse_ShouldNotActivate()
    {
        // Arrange
        var manager = new RegionManager<TargetableRegion>();
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));
        manager.Open<TargetableRegion, string>("context2", ctx => new TargetableRegion(ctx));
        manager.SelectedItem = manager.Items[1]; // Select context2

        // Act
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx), isActive: _ => false);

        // Assert
        manager.SelectedItem.Should().BeSameAs(manager.Items[1], "should stay on context2");

        manager.Dispose();
    }

    [Fact]
    public void Open_WithDifferentContexts_ShouldCreateMultiple()
    {
        // Arrange
        var manager = new RegionManager<TargetableRegion>();

        // Act
        manager.Open<TargetableRegion, string>("context1", ctx => new TargetableRegion(ctx));
        manager.Open<TargetableRegion, string>("context2", ctx => new TargetableRegion(ctx));
        manager.Open<TargetableRegion, string>("context3", ctx => new TargetableRegion(ctx));

        // Assert
        manager.Items.Should().HaveCount(3);
        manager.Items[0].TargetId.Should().Be("context1");
        manager.Items[1].TargetId.Should().Be("context2");
        manager.Items[2].TargetId.Should().Be("context3");

        manager.Dispose();
    }

    #endregion

    #region Close Tests

    [Fact]
    public void Close_Region_ShouldRemoveFromItems()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Test" });
        var region = _manager.Items[0];

        // Act
        region.Close();

        // Assert
        _manager.Items.Should().BeEmpty();
    }

    [Fact]
    public void Close_FirstOfThree_ShouldRemoveAndSelectNext()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });
        var firstRegion = _manager.Items[0];
        _manager.SelectedItem = firstRegion;

        // Act
        firstRegion.Close();

        // Assert
        _manager.Items.Should().HaveCount(2);
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[0]);
        _manager.SelectedItem!.Data.Should().Be("Region2");
    }

    [Fact]
    public void Close_MiddleOfThree_ShouldRemoveAndSelectPrevious()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });
        var middleRegion = _manager.Items[1];
        _manager.SelectedItem = middleRegion;

        // Act
        middleRegion.Close();

        // Assert
        _manager.Items.Should().HaveCount(2);
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[0]);
        _manager.SelectedItem!.Data.Should().Be("Region1");
    }

    [Fact]
    public void Close_LastOfThree_ShouldRemoveAndSelectPrevious()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });
        var lastRegion = _manager.Items[2];
        _manager.SelectedItem = lastRegion;

        // Act
        lastRegion.Close();

        // Assert
        _manager.Items.Should().HaveCount(2);
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[1]);
        _manager.SelectedItem!.Data.Should().Be("Region2");
    }

    [Fact]
    public void Close_OnlyRegion_ShouldRemoveAndSetSelectedToNull()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Only" });
        var region = _manager.Items[0];
        _manager.SelectedItem = region;

        // Act
        region.Close();

        // Assert
        _manager.Items.Should().BeEmpty();
        _manager.SelectedItem.Should().BeNull();
    }

    [Fact]
    public void Close_NonSelectedRegion_ShouldNotChangeSelection()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });
        var regionToClose = _manager.Items[1];
        _manager.SelectedItem = _manager.Items[2]; // Select Region3

        // Act
        regionToClose.Close();

        // Assert
        _manager.Items.Should().HaveCount(2);
        _manager.SelectedItem!.Data.Should().Be("Region3", "selection should not change");
    }

    #endregion

    #region SelectedItem Tests

    [Fact]
    public void SelectedItem_SetManually_ShouldUpdate()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });

        // Act
        _manager.SelectedItem = _manager.Items[1];

        // Assert
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[1]);
        _manager.SelectedItem!.Data.Should().Be("Region2");
    }

    [Fact]
    public void SelectedItem_SetToNull_ShouldBeAllowed()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });

        // Act
        _manager.SelectedItem = null;

        // Assert
        _manager.SelectedItem.Should().BeNull();
    }

    [Fact]
    public void SelectedItem_WhenChanged_ShouldRaisePropertyChanged()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        var eventRaised = false;

        _manager.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(RegionManager<TestRegion>.SelectedItem))
                eventRaised = true;
        };

        // Act
        _manager.SelectedItem = _manager.Items[1];

        // Assert
        eventRaised.Should().BeTrue();
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_ShouldCloseAllRegions()
    {
        // Arrange
        var region1 = new TestRegion { Data = "Region1" };
        var region2 = new TestRegion { Data = "Region2" };
        var region3 = new TestRegion { Data = "Region3" };

        _manager.Open(() => region1);
        _manager.Open(() => region2);
        _manager.Open(() => region3);

        // Act
        _manager.Dispose();

        // Assert
        region1.IsDisposed.Should().BeTrue();
        region2.IsDisposed.Should().BeTrue();
        region3.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Dispose_ShouldClearItems()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });

        // Act
        _manager.Dispose();

        // Assert - Items should be cleared (observable collection is disposed)
        // Note: After disposal, the collection should be empty
        _manager.Items.Should().BeEmpty();
    }

    [Fact]
    public void Dispose_WhenEmpty_ShouldNotThrow()
    {
        // Act
        var act = () => _manager.Dispose();

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Observable Collection Tests

    [Fact]
    public void Items_WhenRegionAdded_ShouldRaiseCollectionChanged()
    {
        // Arrange
        var eventRaised = false;
        NotifyCollectionChangedEventArgs? eventArgs = null;

        _manager.Items.CollectionChanged += (sender, args) =>
        {
            eventRaised = true;
            eventArgs = args;
        };

        // Act
        _manager.Open(() => new TestRegion { Data = "Test" });

        // Give time for observable to synchronize
        System.Threading.Thread.Sleep(100);

        // Assert
        eventRaised.Should().BeTrue();
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Add);
    }

    [Fact]
    public void Items_WhenRegionRemoved_ShouldRaiseCollectionChanged()
    {
        // Arrange
        _manager.Open(() => new TestRegion { Data = "Test" });
        var region = _manager.Items[0];

        var eventRaised = false;
        NotifyCollectionChangedEventArgs? eventArgs = null;

        // Give time for initial add to complete
        System.Threading.Thread.Sleep(100);

        _manager.Items.CollectionChanged += (sender, args) =>
        {
            eventRaised = true;
            eventArgs = args;
        };

        // Act
        region.Close();

        // Give time for observable to synchronize
        System.Threading.Thread.Sleep(100);

        // Assert
        eventRaised.Should().BeTrue();
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Remove);
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public void Open_ConcurrentCalls_ShouldHandleSafely()
    {
        // Arrange
        var tasks = new List<Task>();
        var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

        // Act - Try to open regions concurrently
        for (int i = 0; i < 20; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    _manager.Open(() => new TestRegion { Data = $"Region{index}" });
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Give time for observables to synchronize
        System.Threading.Thread.Sleep(500);

        // Assert
        exceptions.Should().BeEmpty("no exceptions should occur");
        _manager.Items.Should().HaveCount(20, "all regions should be added");
    }

    [Fact]
    public void Close_ConcurrentCalls_ShouldHandleSafely()
    {
        // Arrange - Create 10 regions
        var regions = new List<TestRegion>();
        for (int i = 0; i < 10; i++)
        {
            var region = new TestRegion { Data = $"Region{i}" };
            regions.Add(region);
            _manager.Open(() => region);
        }

        System.Threading.Thread.Sleep(200);

        var tasks = new List<Task>();
        var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

        // Act - Try to close regions concurrently
        foreach (var region in regions)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    region.Close();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Give time for observables to synchronize
        System.Threading.Thread.Sleep(500);

        // Assert
        exceptions.Should().BeEmpty("no exceptions should occur");
        _manager.Items.Should().BeEmpty("all regions should be closed");
    }

    [Fact]
    public void MixedOperations_Concurrent_ShouldHandleSafely()
    {
        // Arrange
        var tasks = new List<Task>();
        var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();
        var createdRegions = new System.Collections.Concurrent.ConcurrentBag<TestRegion>();

        // Act - Mix of opens and closes
        for (int i = 0; i < 30; i++)
        {
            var index = i;

            if (i % 3 == 0)
            {
                // Open task
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        var region = new TestRegion { Data = $"Region{index}" };
                        createdRegions.Add(region);
                        _manager.Open(() => region);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }));
            }
            else if (i % 3 == 1)
            {
                // Selection change task
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        if (_manager.Items.Count > 0)
                        {
                            var randomIndex = Random.Shared.Next(_manager.Items.Count);
                            _manager.SelectedItem = _manager.Items[randomIndex];
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }));
            }
        }

        Task.WaitAll(tasks.ToArray());
        System.Threading.Thread.Sleep(500);

        // Assert
        exceptions.Should().BeEmpty("no exceptions should occur during concurrent operations");
        _manager.Items.Should().NotBeNull();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Open_FirstRegionWithNoSelection_ShouldAutoSelect()
    {
        // This tests the logic: if (SelectedItem is null) SelectedItem = regionModel;

        // Act
        _manager.Open(() => new TestRegion { Data = "First" }, isActive: _ => false);

        // Assert
        _manager.SelectedItem.Should().NotBeNull("first region should auto-select even with isActive=false if no selection exists");
        _manager.SelectedItem!.Data.Should().Be("First");
    }

    [Fact]
    public void Close_UpdatesSelectionIndexCorrectly_WhenClosingAtIndex0()
    {
        // This tests the edge case: var newIndex = index == 0 ? 0 : index - 1;
        // When closing index 0, newIndex should be 0

        // Arrange
        _manager.Open(() => new TestRegion { Data = "Region1" });
        _manager.Open(() => new TestRegion { Data = "Region2" });
        _manager.Open(() => new TestRegion { Data = "Region3" });
        var firstRegion = _manager.Items[0];
        _manager.SelectedItem = firstRegion;

        // Act
        firstRegion.Close();

        // Assert
        _manager.SelectedItem.Should().BeSameAs(_manager.Items[0], "should select item at index 0 (which is now Region2)");
    }

    #endregion
}
