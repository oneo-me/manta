using System.Text.RegularExpressions;
using Manta.Themes.Values;
using Manta.Tools.Values;

namespace Manta.Tools.Extensions;

static partial class ValueExtensions
{
    [GeneratedRegex(@"hsl\((\d+)\s+(\d+%)\s+(\d+%)\s*/\s*([\d.]+)\)")]
    private static partial Regex HslRegex();

    extension(string value)
    {
        public NumberValue ParserNumber()
        {
            // Example: 0.5rem
            const string remSuffix = "rem";

            if (value.EndsWith(remSuffix))
            {
                var numberPart = value[..^remSuffix.Length];
                if (double.TryParse(numberPart, out var number))
                    return new()
                    {
                        Value = number * 16 // Assuming 1rem = 16px
                    };
            }

            throw new FormatException($"Invalid rem value format: {value}");
        }

        public ShadowValue ParseShadow()
        {
            var converted = HslRegex().Replace(value, match =>
            {
                var h = match.Groups[1].Value;
                var s = match.Groups[2].Value;
                var l = match.Groups[3].Value;
                var a = match.Groups[4].Value;

                if (h == "0" && s == "0%" && l == "0%")
                {
                    var alpha = (int)(double.Parse(a) * 255);
                    return $"#{alpha:X2}000000";
                }

                return $"hsla({h}, {s}, {l}, {a})";
            });

            return new() { Value = converted.Replace("px", "") };
        }

        public OklchColorValue ParserColor()
        {
            // Example: oklch(0.927 0.123 250)
            const string prefix = "oklch(";
            const string suffix = ")";

            if (value.StartsWith(prefix) && value.EndsWith(suffix))
            {
                var content = value[prefix.Length..^suffix.Length];
                var parts = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3 &&
                    double.TryParse(parts[0], out var l) &&
                    double.TryParse(parts[1], out var c) &&
                    double.TryParse(parts[2], out var h))
                    return new(new()
                    {
                        L = l,
                        C = c,
                        H = h
                    });
            }

            // Example: #000000
            if (value.StartsWith('#'))
            {
                var hex = value[1..];
                if (hex.Length is 6 or 8)
                {
                    var r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    var g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    var b = Convert.ToInt32(hex.Substring(4, 2), 16);

                    return new(new RgbColor
                    {
                        R = r,
                        G = g,
                        B = b
                    }.ToOklchColor());
                }
            }

            throw new FormatException($"Invalid oklch color format: {value}");
        }
    }
}
