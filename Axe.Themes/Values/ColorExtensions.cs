namespace Axe.Themes.Values;

static class ColorExtensions
{
    static readonly double[,] _srgbToLinearMatrix =
    {
        { 0.4124564, 0.3575761, 0.1804375 },
        { 0.2126729, 0.7151522, 0.0721750 },
        { 0.0193339, 0.1191920, 0.9503041 }
    };

    static readonly double[,] _linearToSrgbMatrix =
    {
        { 3.2404542, -1.5371385, -0.4985314 },
        { -0.9692660, 1.8760108, 0.0415560 },
        { 0.0556434, -0.2040259, 1.0572252 }
    };

    public static OklchColor ToOklchColor(this RgbColor rgbColor)
    {
        var rLinear = SrgbToLinear(rgbColor.R / 255.0);
        var gLinear = SrgbToLinear(rgbColor.G / 255.0);
        var bLinear = SrgbToLinear(rgbColor.B / 255.0);

        var x = _srgbToLinearMatrix[0, 0] * rLinear + _srgbToLinearMatrix[0, 1] * gLinear + _srgbToLinearMatrix[0, 2] * bLinear;
        var y = _srgbToLinearMatrix[1, 0] * rLinear + _srgbToLinearMatrix[1, 1] * gLinear + _srgbToLinearMatrix[1, 2] * bLinear;
        var z = _srgbToLinearMatrix[2, 0] * rLinear + _srgbToLinearMatrix[2, 1] * gLinear + _srgbToLinearMatrix[2, 2] * bLinear;

        var l = Math.Pow(0.8189330101 * x + 0.3618667424 * y - 0.1288597137 * z, 1.0 / 3.0);
        var m = Math.Pow(0.0329845436 * x + 0.9293118715 * y + 0.0361456387 * z, 1.0 / 3.0);
        var s = Math.Pow(0.0482003018 * x + 0.2643662691 * y + 0.6338517070 * z, 1.0 / 3.0);

        var oklabL = 0.2104542553 * l + 0.7936177850 * m - 0.0040720468 * s;
        var oklabA = 1.9779984951 * l - 2.4285922050 * m + 0.4505937099 * s;
        var oklabB = 0.0259040371 * l + 0.7827717662 * m - 0.8086757660 * s;

        var oklchL = oklabL;
        var oklchC = Math.Sqrt(oklabA * oklabA + oklabB * oklabB);
        var oklchH = Math.Atan2(oklabB, oklabA) * 180.0 / Math.PI;

        if (oklchH < 0) oklchH += 360;

        return new()
        {
            L = oklchL,
            C = oklchC,
            H = oklchH
        };
    }

    public static RgbColor ToRgbColor(this OklchColor oklchColor)
    {
        var hRad = oklchColor.H * Math.PI / 180.0;
        var oklabL = oklchColor.L;
        var oklabA = oklchColor.C * Math.Cos(hRad);
        var oklabB = oklchColor.C * Math.Sin(hRad);

        var lCube = oklabL + 0.3963377774 * oklabA + 0.2158037573 * oklabB;
        var mCube = oklabL - 0.1055613458 * oklabA - 0.0638541728 * oklabB;
        var sCube = oklabL - 0.0894841775 * oklabA - 1.2914855480 * oklabB;

        var lLinear = lCube * lCube * lCube;
        var mLinear = mCube * mCube * mCube;
        var sLinear = sCube * sCube * sCube;

        var x = 1.2270138511 * lLinear - 0.5577999807 * mLinear + 0.2812561490 * sLinear;
        var y = -0.0405801784 * lLinear + 1.1122568696 * mLinear - 0.0716766787 * sLinear;
        var z = -0.0763812845 * lLinear - 0.4214819784 * mLinear + 1.5861632204 * sLinear;

        var rLinear = _linearToSrgbMatrix[0, 0] * x + _linearToSrgbMatrix[0, 1] * y + _linearToSrgbMatrix[0, 2] * z;
        var gLinear = _linearToSrgbMatrix[1, 0] * x + _linearToSrgbMatrix[1, 1] * y + _linearToSrgbMatrix[1, 2] * z;
        var bLinear = _linearToSrgbMatrix[2, 0] * x + _linearToSrgbMatrix[2, 1] * y + _linearToSrgbMatrix[2, 2] * z;

        var r = (int)Math.Round(LinearToSrgb(rLinear) * 255);
        var g = (int)Math.Round(LinearToSrgb(gLinear) * 255);
        var b = (int)Math.Round(LinearToSrgb(bLinear) * 255);

        r = Math.Max(0, Math.Min(255, r));
        g = Math.Max(0, Math.Min(255, g));
        b = Math.Max(0, Math.Min(255, b));

        return new()
        {
            R = r,
            G = g,
            B = b
        };
    }

    static double SrgbToLinear(double value)
    {
        if (value <= 0.04045)
            return value / 12.92;
        return Math.Pow((value + 0.055) / 1.055, 2.4);
    }

    static double LinearToSrgb(double value)
    {
        if (value <= 0.0031308)
            return value * 12.92;
        return 1.055 * Math.Pow(value, 1.0 / 2.4) - 0.055;
    }
}
