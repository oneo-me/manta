using Avalonia.Media;
using Manta.Themes.Values;

namespace Manta.Tests.Themes.Values;

/// <summary>
/// Tests for RgbColor (Manta/Themes/Values/RgbColor.cs)
/// Priority: LOW - Simple color conversion
/// </summary>
public class RgbColorTests
{
    #region ToColor Tests - Basic

    [Fact]
    public void ToColor_WithDefaultOpacity_ShouldReturnCorrectColor()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.A.Should().Be(255);
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_BlackColor_ShouldReturnBlack()
    {
        // Arrange
        var rgb = new RgbColor { R = 0, G = 0, B = 0 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.Should().Be(Color.FromArgb(255, 0, 0, 0));
    }

    [Fact]
    public void ToColor_WhiteColor_ShouldReturnWhite()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 255, B = 255 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.Should().Be(Color.FromArgb(255, 255, 255, 255));
    }

    [Fact]
    public void ToColor_RedColor_ShouldReturnRed()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 0, B = 0 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.Should().Be(Color.FromArgb(255, 255, 0, 0));
    }

    [Fact]
    public void ToColor_GreenColor_ShouldReturnGreen()
    {
        // Arrange
        var rgb = new RgbColor { R = 0, G = 255, B = 0 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.Should().Be(Color.FromArgb(255, 0, 255, 0));
    }

    [Fact]
    public void ToColor_BlueColor_ShouldReturnBlue()
    {
        // Arrange
        var rgb = new RgbColor { R = 0, G = 0, B = 255 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.Should().Be(Color.FromArgb(255, 0, 0, 255));
    }

    #endregion

    #region ToColor Tests - Opacity Factor

    [Fact]
    public void ToColor_WithOpacityFactorHalf_ShouldApplyCorrectAlpha()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64 };

        // Act
        var result = rgb.ToColor(0.5);

        // Assert
        result.A.Should().Be(127); // 255 * 1.0 * 0.5 ≈ 127.5 → 127
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_WithOpacityFactorZero_ShouldBeFullyTransparent()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64 };

        // Act
        var result = rgb.ToColor(0.0);

        // Assert
        result.A.Should().Be(0);
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_WithOpacityFactorOne_ShouldBeFullyOpaque()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64 };

        // Act
        var result = rgb.ToColor(1.0);

        // Assert
        result.A.Should().Be(255);
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_WithOpacityFactorQuarter_ShouldApplyCorrectAlpha()
    {
        // Arrange
        var rgb = new RgbColor { R = 200, G = 100, B = 50 };

        // Act
        var result = rgb.ToColor(0.25);

        // Assert
        result.A.Should().Be(63); // 255 * 1.0 * 0.25 ≈ 63.75 → 63
        result.R.Should().Be(200);
        result.G.Should().Be(100);
        result.B.Should().Be(50);
    }

    #endregion

    #region ToColor Tests - Base Opacity

    [Fact]
    public void ToColor_WithBaseOpacity_ShouldApplyToAlpha()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64, Opacity = 0.5 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.A.Should().Be(127); // 255 * 0.5 ≈ 127.5 → 127
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_WithBaseOpacityAndFactor_ShouldMultiplyBoth()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64, Opacity = 0.5 };

        // Act
        var result = rgb.ToColor(0.5);

        // Assert
        result.A.Should().Be(63); // 255 * 0.5 * 0.5 ≈ 63.75 → 63
        result.R.Should().Be(255);
        result.G.Should().Be(128);
        result.B.Should().Be(64);
    }

    [Fact]
    public void ToColor_WithZeroBaseOpacity_ShouldBeTransparent()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 128, B = 64, Opacity = 0.0 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.A.Should().Be(0);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ToColor_WithMaxValues_ShouldHandleCorrectly()
    {
        // Arrange
        var rgb = new RgbColor { R = 255, G = 255, B = 255, Opacity = 1.0 };

        // Act
        var result = rgb.ToColor(1.0);

        // Assert
        result.Should().Be(Color.FromArgb(255, 255, 255, 255));
    }

    [Fact]
    public void ToColor_WithMinValues_ShouldHandleCorrectly()
    {
        // Arrange
        var rgb = new RgbColor { R = 0, G = 0, B = 0, Opacity = 0.0 };

        // Act
        var result = rgb.ToColor(0.0);

        // Assert
        result.Should().Be(Color.FromArgb(0, 0, 0, 0));
    }

    [Fact]
    public void ToColor_WithMixedOpacities_ShouldCalculateCorrectly()
    {
        // Arrange
        var rgb = new RgbColor { R = 200, G = 150, B = 100, Opacity = 0.8 };

        // Act
        var result = rgb.ToColor(0.75);

        // Assert
        var expectedAlpha = (byte)(255.0 * 0.8 * 0.75); // = 153
        result.A.Should().Be(expectedAlpha);
        result.R.Should().Be(200);
        result.G.Should().Be(150);
        result.B.Should().Be(100);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void RgbColor_Properties_ShouldBeInitializable()
    {
        // Act
        var rgb = new RgbColor
        {
            R = 100,
            G = 150,
            B = 200,
            Opacity = 0.7
        };

        // Assert
        rgb.R.Should().Be(100);
        rgb.G.Should().Be(150);
        rgb.B.Should().Be(200);
        rgb.Opacity.Should().Be(0.7);
    }

    [Fact]
    public void RgbColor_DefaultOpacity_ShouldBeInherited()
    {
        // Note: Opacity is inherited from ColorBase, default should be 1.0
        // Arrange
        var rgb = new RgbColor { R = 255, G = 0, B = 0 };

        // Act
        var result = rgb.ToColor();

        // Assert
        result.A.Should().Be(255, "default opacity should be 1.0");
    }

    #endregion
}
