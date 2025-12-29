# Manta Tests

This project contains comprehensive unit tests for the Manta library.

## Test Coverage

### High Priority (Critical Infrastructure)

- **ServiceTests.cs** - Service container and dependency injection
  - 20+ test cases covering registration, retrieval, and creation
  - Thread safety tests
  - Error handling validation

- **ViewManagerTests.cs** - View lifecycle and navigation management
  - 25+ test cases covering view creation, disposal, and switching
  - Memory leak detection tests
  - Property change notifications

- **RegionManagerTests.cs** - Region/tab management with observables
  - 30+ test cases covering region opening, closing, and selection
  - Thread safety with concurrent operations
  - Observable collection synchronization
  - Edge case handling for selection logic

### Medium Priority (Supporting Systems)

- **LogTests.cs** - Logging system
  - Message formatting and template parsing
  - Multiple logger distribution
  - Thread safety tests
  - All log levels (Verbose, Debug, Info, Warning, Error, Fatal)

### Low Priority (UI Components)

- **Number2ThicknessConverterTests.cs** - Value converter
  - All numeric type conversions
  - Invalid type handling
  - Edge cases (zero, negative, large values)

- **RgbColorTests.cs** - Color system
  - RGB to Color conversion
  - Opacity calculations
  - Edge cases and property tests

## Running Tests

```bash
dotnet test
```

## Running Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

## Test Statistics

- **Total Test Files**: 6
- **Total Test Cases**: 150+
- **Estimated Coverage**: 75%+ of core functionality

## Test Organization

```
Manta.Tests/
├── Reactive/
│   ├── Services/
│   │   └── ServiceTests.cs
│   ├── Views/
│   │   └── ViewManagerTests.cs
│   └── Regions/
│       └── RegionManagerTests.cs
├── Logging/
│   └── LogTests.cs
├── Converters/
│   └── Number2ThicknessConverterTests.cs
└── Themes/
    └── Values/
        └── RgbColorTests.cs
```

## Technologies Used

- **xUnit** - Test framework
- **FluentAssertions** - Assertion library
- **NSubstitute** - Mocking framework (ready for use)
- **Coverlet** - Code coverage tool

## Next Steps

See [TEST_COVERAGE_ANALYSIS.md](../TEST_COVERAGE_ANALYSIS.md) for:
- Additional areas requiring tests
- Implementation roadmap
- Recommended testing practices
- CI/CD integration guidance
