using Avalonia;
using Avalonia.Data.Converters;
using Manta.Converters;
using System.Globalization;

namespace Manta.Tests.Converters;

/// <summary>
/// Tests for Number2ThicknessConverter (Manta/Converters/Number2ThicknessConverter.cs)
/// Priority: LOW - Simple value converter
/// </summary>
public class Number2ThicknessConverterTests
{
    private readonly Number2ThicknessConverter _converter;
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    public Number2ThicknessConverterTests()
    {
        _converter = new Number2ThicknessConverter();
    }

    #region Convert Tests - Valid Types

    [Fact]
    public void Convert_IntValue_ShouldReturnThickness()
    {
        // Arrange
        int value = 10;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(10));
    }

    [Fact]
    public void Convert_DoubleValue_ShouldReturnThickness()
    {
        // Arrange
        double value = 15.5;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(15.5));
    }

    [Fact]
    public void Convert_FloatValue_ShouldReturnThickness()
    {
        // Arrange
        float value = 12.3f;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(12.3));
    }

    [Fact]
    public void Convert_LongValue_ShouldReturnThickness()
    {
        // Arrange
        long value = 20L;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(20));
    }

    [Fact]
    public void Convert_ShortValue_ShouldReturnThickness()
    {
        // Arrange
        short value = 8;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(8));
    }

    [Fact]
    public void Convert_ByteValue_ShouldReturnThickness()
    {
        // Arrange
        byte value = 5;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(5));
    }

    #endregion

    #region Convert Tests - Edge Cases

    [Fact]
    public void Convert_ZeroValue_ShouldReturnZeroThickness()
    {
        // Arrange
        int value = 0;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(0));
    }

    [Fact]
    public void Convert_NegativeValue_ShouldReturnNegativeThickness()
    {
        // Arrange
        int value = -10;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(-10));
    }

    [Fact]
    public void Convert_VeryLargeValue_ShouldReturnThickness()
    {
        // Arrange
        double value = double.MaxValue;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(double.MaxValue));
    }

    #endregion

    #region Convert Tests - Invalid Types

    [Fact]
    public void Convert_NullValue_ShouldReturnUnsetValue()
    {
        // Act
        var result = _converter.Convert(null, typeof(Thickness), null, _culture);

        // Assert
        result.Should().Be(AvaloniaProperty.UnsetValue);
    }

    [Fact]
    public void Convert_StringValue_ShouldReturnUnsetValue()
    {
        // Arrange
        string value = "10";

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().Be(AvaloniaProperty.UnsetValue);
    }

    [Fact]
    public void Convert_BoolValue_ShouldReturnUnsetValue()
    {
        // Arrange
        bool value = true;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().Be(AvaloniaProperty.UnsetValue);
    }

    [Fact]
    public void Convert_ObjectValue_ShouldReturnUnsetValue()
    {
        // Arrange
        object value = new object();

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().Be(AvaloniaProperty.UnsetValue);
    }

    [Fact]
    public void Convert_DecimalValue_ShouldReturnUnsetValue()
    {
        // Arrange - decimal is not handled
        decimal value = 10.5m;

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, _culture);

        // Assert
        result.Should().Be(AvaloniaProperty.UnsetValue);
    }

    #endregion

    #region ConvertBack Tests

    [Fact]
    public void ConvertBack_ShouldThrowNotImplementedException()
    {
        // Arrange
        var thickness = new Thickness(10);

        // Act
        var act = () => _converter.ConvertBack(thickness, typeof(int), null, _culture);

        // Assert
        act.Should().Throw<NotImplementedException>();
    }

    #endregion

    #region Parameter and Culture Tests

    [Fact]
    public void Convert_WithParameter_ShouldIgnoreParameter()
    {
        // Arrange
        int value = 10;
        object parameter = "ignored";

        // Act
        var result = _converter.Convert(value, typeof(Thickness), parameter, _culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(10));
    }

    [Fact]
    public void Convert_WithDifferentCulture_ShouldWorkCorrectly()
    {
        // Arrange
        double value = 10.5;
        var culture = new CultureInfo("de-DE"); // Uses comma as decimal separator

        // Act
        var result = _converter.Convert(value, typeof(Thickness), null, culture);

        // Assert
        result.Should().BeOfType<Thickness>();
        ((Thickness)result).Should().Be(new Thickness(10.5));
    }

    #endregion
}
