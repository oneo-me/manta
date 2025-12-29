# Test Coverage Analysis for Manta

**Date**: December 29, 2025
**Analyzed by**: Claude
**Branch**: claude/analyze-test-coverage-YnYvw

## Executive Summary

**Critical Finding**: The Manta codebase currently has **zero test coverage** - no test projects exist in the repository.

- **Total C# Files**: 152
- **Projects**: 4 (Manta, Manta.Generators, Manta.Demo, Manta.Prepare)
- **Test Coverage**: 0%
- **Risk Level**: HIGH

## Current State

The Manta project is an Avalonia UI framework providing:
- Reactive MVVM infrastructure
- Service container/dependency injection
- View and region management
- Custom UI controls and layouts
- Globalization system
- Logging framework
- Theme system
- Source generators

## Critical Areas Requiring Test Coverage

### 1. Service Container System ⚠️ HIGH PRIORITY

**Location**: `Manta/Reactive/Services/Service.cs`

**Risk**: Core dependency injection used throughout the application. Bugs here affect the entire system.

**Key Concerns**:
- Thread safety of static dictionary
- Service registration validation
- Dependency resolution logic
- Error handling for edge cases

**Recommended Test Coverage**:
```
✓ Service registration (single type)
✓ Service registration (interface + implementation)
✓ Service retrieval with GetService<T>()
✓ Constructor dependency injection via CreateInstance<T>()
✓ Duplicate registration error handling (line 20-21)
✓ Unregistered service retrieval error
✓ Type hierarchy validation (line 17-18)
✓ Multiple constructor selection logic (line 60)
✓ Thread safety tests
```

**Bug Risks**:
- Line 21: Potential KeyNotFoundException if serviceType not in dictionary
- Line 70: Unhandled exceptions from GetInstance
- Thread safety not verified

### 2. View Management System ⚠️ HIGH PRIORITY

**Location**: `Manta/Reactive/Views/ViewManager.cs`

**Risk**: Manages view lifecycle and navigation. Memory leaks or disposal issues could accumulate.

**Key Concerns**:
- Proper disposal of views (line 18)
- Memory leaks from undisposed ViewModels
- Service creation integration

**Recommended Test Coverage**:
```
✓ OpenView with factory function
✓ OpenView with service resolution
✓ OpenView by Type
✓ ClearView disposes current view
✓ Multiple OpenView calls properly dispose previous views
✓ Disposal cleanup
✓ Memory leak tests
```

### 3. Region Manager ⚠️ HIGH PRIORITY

**Location**: `Manta/Reactive/Regions/RegionManager<T>.cs`

**Risk**: Complex threading logic with Lock. Tab/region management is critical for multi-document interfaces.

**Key Concerns**:
- Thread safety with Lock (lines 12, 39, 68, 93, 108)
- DynamicData observable collection synchronization
- Selection state management
- Disposal of region items

**Recommended Test Coverage**:
```
✓ Open new region
✓ Find existing region by IRegionTarget<TContext>
✓ Activate region (SelectedItem assignment)
✓ Close region and selection update logic (lines 95-103)
✓ Close last region (SelectedItem should be null)
✓ Close middle region (selection index calculation)
✓ Thread safety (concurrent Open/Close operations)
✓ Disposal cascades to all regions (lines 110-111)
✓ Observable collection synchronization
```

**Bug Risks**:
- Line 102: Index calculation when closing - potential off-by-one errors
- Race conditions in Find + Create scenario

### 4. Language Key Generator ⚠️ MEDIUM PRIORITY

**Location**: `Manta.Generators/LangKey/LangKeyGenerator.cs`

**Risk**: Source generator errors break the build. Complex YAML parsing logic.

**Key Concerns**:
- YAML parsing correctness (lines 102-127)
- Diagnostic accuracy for missing/redundant keys
- Generated code format
- Config file handling

**Recommended Test Coverage**:
```
✓ Parse valid YAML language files
✓ Detect missing keys across languages (lines 129-134)
✓ Detect redundant keys (lines 137-142)
✓ Handle @config.yml files
✓ Generate correct C# code format
✓ Handle empty files
✓ Handle malformed YAML
✓ Location tracking for diagnostics
```

**Integration Tests**:
- End-to-end generator execution
- Multiple language file scenarios

### 5. Logging System ⚠️ MEDIUM PRIORITY

**Location**: `Manta/Logging/Log.cs`

**Risk**: Logging failures could hide critical errors in production.

**Key Concerns**:
- Message template parsing (lines 95-131)
- Thread safety of logger collection (line 9, 14, 23)
- Format string handling with `{}` and `{{}}`

**Recommended Test Coverage**:
```
✓ AddLogger thread safety
✓ WriteLine distributes to all loggers
✓ Log level formatting (lines 78-93)
✓ Time formatting (lines 60-76)
✓ Message template parsing with parameters
✓ Handle escaped braces {{}}
✓ Handle missing parameters
✓ CharacterReader edge cases
```

### 6. Dialog System 🔵 MEDIUM PRIORITY

**Location**: `Manta/Dialogs/MDialog.cs`

**Risk**: User interaction correctness. Relatively simple but critical for UX.

**Recommended Test Coverage**:
```
✓ Alert shows confirm button only (lines 33-37)
✓ Confirm shows both buttons (lines 40-44)
✓ Return values are correct
✓ Options are applied correctly
```

### 7. Color System 🟢 LOW PRIORITY

**Location**: `Manta/Themes/Values/RgbColor.cs`

**Risk**: Low - simple mathematical conversion.

**Recommended Test Coverage**:
```
✓ ToColor with various RGB values
✓ Opacity factor calculation (line 13)
✓ Byte conversion from int (line 13)
✓ Edge cases (0, 255)
```

### 8. Number Converter 🟢 LOW PRIORITY

**Location**: `Manta/Converters/Number2ThicknessConverter.cs`

**Risk**: Low - straightforward value converter.

**Recommended Test Coverage**:
```
✓ Convert int, double, float, long, short, byte (lines 11-22)
✓ Invalid types return UnsetValue (line 24)
✓ ConvertBack throws NotImplementedException (line 29)
```

## Recommended Test Project Structure

```
Solution Root/
├── Manta/
├── Manta.Generators/
├── Manta.Demo/
├── Manta.Prepare/
├── Manta.Tests/                          [NEW]
│   ├── Manta.Tests.csproj
│   ├── Reactive/
│   │   ├── Services/
│   │   │   ├── ServiceTests.cs          [Priority 1]
│   │   │   └── ServiceExceptionTests.cs
│   │   ├── Views/
│   │   │   ├── ViewManagerTests.cs      [Priority 1]
│   │   │   └── ViewModelTests.cs
│   │   ├── Regions/
│   │   │   └── RegionManagerTests.cs    [Priority 1]
│   │   ├── Commands/
│   │   │   └── CommandWarperTests.cs
│   │   └── Extensions/
│   │       └── ObservableExtensionsTests.cs
│   ├── Logging/
│   │   ├── LogTests.cs                  [Priority 2]
│   │   └── Utilities/
│   │       ├── CharacterReaderTests.cs
│   │       └── StringBuilderCacheTests.cs
│   ├── Converters/
│   │   └── Number2ThicknessConverterTests.cs
│   ├── Themes/
│   │   └── Values/
│   │       └── RgbColorTests.cs
│   ├── Dialogs/
│   │   └── MDialogTests.cs              [Priority 2]
│   └── TestHelpers/
│       ├── MockLogger.cs
│       └── TestSchedulers.cs
└── Manta.Generators.Tests/              [NEW]
    ├── Manta.Generators.Tests.csproj
    └── LangKey/
        ├── LangKeyGeneratorTests.cs     [Priority 2]
        └── TestData/
            ├── en.yml
            ├── zh.yml
            └── @config.yml
```

## Testing Technology Recommendations

### Unit Testing Framework
**Recommendation**: **xUnit**
- Industry standard for .NET
- Excellent async support
- Extensible architecture
- Better isolation between tests

Alternative: NUnit (if team prefers)

### Mocking Framework
**Recommendation**: **NSubstitute**
- Cleaner syntax than Moq
- Better error messages
- Simpler learning curve

```csharp
// Example for ILogger mocking
var logger = Substitute.For<ILogger>();
Log.AddLogger(logger);
Log.Information("Test");
logger.Received(1).WriteLine(Arg.Any<DateTimeOffset>(), LogLevel.Information, "Test", Arg.Any<object[]>());
```

### Avalonia UI Testing
**Recommendation**: **Avalonia.Headless**
- Official Avalonia testing package
- No window manager required
- Runs in CI/CD environments

### Source Generator Testing
**Recommendation**: **Microsoft.CodeAnalysis.CSharp.SourceGenerators.Testing.XUnit**
- Official Roslyn testing package
- Verifies generated code
- Validates diagnostics

Example:
```csharp
await new CSharpSourceGeneratorTest<LangKeyGenerator, XUnitVerifier>
{
    TestState =
    {
        AdditionalFiles = { ("Langs/en.yml", langYaml) },
        GeneratedSources = { (typeof(LangKeyGenerator), "LangKey.g.cs", expectedCode) }
    }
}.RunAsync();
```

### Code Coverage
**Recommendation**: **Coverlet + ReportGenerator**
- Integrated with dotnet test
- Multiple report formats
- CI/CD friendly

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport
```

## Implementation Roadmap

### Phase 1: Foundation (Week 1-2)
**Goal**: Test critical infrastructure

1. Create Manta.Tests project
2. Implement ServiceTests.cs (100% coverage)
   - All registration scenarios
   - All retrieval scenarios
   - All error conditions
   - Thread safety tests
3. Implement ViewManagerTests.cs (100% coverage)
4. Implement RegionManagerTests.cs (80%+ coverage)
5. Set up CI pipeline

**Deliverables**:
- 3 core test classes
- 150+ test cases
- ~60% overall coverage

### Phase 2: Supporting Systems (Week 3-4)
**Goal**: Test observable behavior and diagnostics

1. Implement LogTests.cs
2. Implement CharacterReaderTests.cs
3. Create Manta.Generators.Tests project
4. Implement LangKeyGeneratorTests.cs
5. Implement MDialogTests.cs

**Deliverables**:
- Source generator tests
- Logging system coverage
- ~75% overall coverage

### Phase 3: UI Layer (Week 5-6)
**Goal**: Complete remaining components

1. Implement converter tests
2. Implement color system tests
3. Add integration tests in Manta.Demo.Tests
4. Performance benchmarks

**Deliverables**:
- Full UI component coverage
- Integration test suite
- 80%+ overall coverage

### Phase 4: Quality & Automation (Week 7+)
**Goal**: Establish testing culture

1. Code coverage enforcement (80% minimum)
2. Mutation testing (Stryker.NET)
3. Performance regression tests
4. Documentation of testing patterns

## CI/CD Integration

### GitHub Actions Workflow
```yaml
name: Test

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet restore
      - run: dotnet build --no-restore
      - run: dotnet test --no-build --verbosity normal /p:CollectCoverage=true
      - uses: codecov/codecov-action@v4
```

### Quality Gates
- Minimum 80% code coverage for new code
- All tests must pass before merge
- No high-priority warnings from static analysis

## Specific Test Examples

### Example 1: Service Container Bug Detection

**Potential Bug in Service.cs:21**:
```csharp
if (_dic.ContainsKey(serviceType) || _dic.ContainsKey(serviceImplType))
    throw new ServiceException($"Service registration error, {serviceType} has already been registered as {_dic[serviceType].GetType()}");
```

**Issue**: If `serviceImplType` exists but `serviceType` doesn't, this will throw `KeyNotFoundException` instead of `ServiceException`.

**Test to catch this**:
```csharp
[Fact]
public void Register_DuplicateImplementationType_ShouldThrowServiceException()
{
    // Arrange
    var impl1 = new TestService();
    var impl2 = new TestService();
    Service.Register(impl1);

    // Act & Assert - Should throw ServiceException, not KeyNotFoundException
    var ex = Assert.Throws<ServiceException>(() => Service.Register(impl2));
    Assert.Contains("already been registered", ex.Message);
}
```

### Example 2: Region Manager Selection Logic

**Complex logic in RegionManager.cs:95-103**:
```csharp
void InnerClose(T regionModel)
{
    lock (_lock)
    {
        var index = _source.Items.IndexOf(regionModel);
        _source.Remove(regionModel);

        if (SelectedItem != null && SelectedItem != regionModel)
            return;

        var newIndex = index == 0 ? 0 : index - 1;
        SelectedItem = _source.Count > 0 ? _source.Items[newIndex] : null;
    }
}
```

**Tests needed**:
```csharp
[Fact]
public void Close_FirstTab_ShouldSelectSecondTab() { }

[Fact]
public void Close_MiddleTab_ShouldSelectPreviousTab() { }

[Fact]
public void Close_LastTab_ShouldSelectPreviousTab() { }

[Fact]
public void Close_OnlyTab_ShouldHaveNoSelection() { }

[Fact]
public void Close_NonSelectedTab_ShouldNotChangeSelection() { }
```

### Example 3: Log Message Template Parsing

**Complex parsing logic in Log.cs:95-131**:

**Edge cases to test**:
```csharp
[Theory]
[InlineData("Hello {0}", new object[] { "World" }, "Hello 'World'")]
[InlineData("Hello {{0}}", new object[] { "World" }, "Hello {0}")]  // Escaped
[InlineData("{0} {1}", new object[] { 1, 2 }, "'1' '2'")]
[InlineData("{0} {1}", new object[] { 1 }, "'1' {1}")]  // Missing parameter
[InlineData("[{area}] {0}", new object[] { "msg" }, "  '[test]' 'msg'")]  // With area
public void FormatMessage_EdgeCases_ShouldHandleCorrectly(
    string template, object[] args, string expected)
{
    var result = Log.FormatMessage("test", template, args);
    Assert.Equal(expected, result);
}
```

## Risk Assessment

### Without Tests
- ❌ Refactoring is dangerous (no safety net)
- ❌ Bugs discovered late (production issues)
- ❌ Difficult to onboard new developers
- ❌ Fear of breaking changes slows development
- ❌ Memory leaks undetected (ViewManager, RegionManager)
- ❌ Thread safety issues surface randomly
- ❌ Source generator bugs break builds

### With Tests
- ✅ Confident refactoring
- ✅ Fast feedback on changes
- ✅ Clear usage examples (tests as documentation)
- ✅ Catch regressions immediately
- ✅ Verify thread safety
- ✅ Validate memory management
- ✅ Professional code quality

## Estimated Effort

- **Phase 1**: 40-60 hours (critical infrastructure)
- **Phase 2**: 30-40 hours (supporting systems)
- **Phase 3**: 20-30 hours (remaining components)
- **Phase 4**: 10-20 hours (automation & quality)

**Total**: 100-150 hours to achieve comprehensive coverage

**Recommendation**: Assign 1-2 developers for 4-6 weeks

## Immediate Action Items

1. ✅ **Create Manta.Tests project**
   ```bash
   dotnet new xunit -n Manta.Tests
   cd Manta.Tests
   dotnet add reference ../Manta/Manta.csproj
   dotnet add package NSubstitute
   dotnet add package FluentAssertions
   ```

2. ✅ **Write first critical test**: ServiceTests.cs

3. ✅ **Set up GitHub Actions**: Add test workflow

4. ✅ **Establish coverage baseline**: Run first coverage report

5. ✅ **Document testing patterns**: Create testing guidelines

## Conclusion

The Manta project has **zero test coverage**, which represents a significant technical debt and risk. The core infrastructure (Service container, ViewManager, RegionManager) handles critical functionality including:
- Dependency injection
- Memory management
- Thread synchronization
- UI navigation

**Recommendation**: Prioritize Phase 1 immediately. The Service container is used throughout the codebase, making it the highest-risk component. Starting with comprehensive tests here will provide immediate value and establish testing patterns for the rest of the codebase.

---

*This analysis was generated on December 29, 2025. The recommendations are based on industry best practices for .NET/Avalonia applications.*
