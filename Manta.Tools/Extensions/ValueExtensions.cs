using Manta.Themes.Values;
using Manta.Tools.Values;

namespace Manta.Tools.Extensions;

static class ValueExtensions
{
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

        public OklchColorValue ParserColor()
        {
            // Example: hsl(0 0% 0%)
            if (value is "hsl(0 0% 0%)")
                return new(new()
                {
                    L = 0,
                    C = 0,
                    H = 0
                });

            // Example: oklch(0.927 0.123 250)
            const string oklchPrefix = "oklch(";
            const string oklchSuffix = ")";

            if (value.StartsWith(oklchPrefix) && value.EndsWith(oklchSuffix))
            {
                var content = value[oklchPrefix.Length..^oklchSuffix.Length];
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
