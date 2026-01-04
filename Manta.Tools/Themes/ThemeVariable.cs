using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Manta.Tools.Values;

namespace Manta.Tools.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class ThemeVariable : IEnumerable<string?>
{
    public OklchColorValue? Background { get; set; }
    public OklchColorValue? Foreground { get; set; }
    public OklchColorValue? Card { get; set; }
    public OklchColorValue? CardForeground { get; set; }
    public OklchColorValue? Popover { get; set; }
    public OklchColorValue? PopoverForeground { get; set; }
    public OklchColorValue? Primary { get; set; }
    public OklchColorValue? PrimaryForeground { get; set; }
    public OklchColorValue? Secondary { get; set; }
    public OklchColorValue? SecondaryForeground { get; set; }
    public OklchColorValue? Muted { get; set; }
    public OklchColorValue? MutedForeground { get; set; }
    public OklchColorValue? Accent { get; set; }
    public OklchColorValue? AccentForeground { get; set; }
    public OklchColorValue? Destructive { get; set; }
    public OklchColorValue? DestructiveForeground { get; set; }
    public OklchColorValue? Border { get; set; }
    public OklchColorValue? Input { get; set; }
    public OklchColorValue? Ring { get; set; }
    public OklchColorValue? Chart1 { get; set; }
    public OklchColorValue? Chart2 { get; set; }
    public OklchColorValue? Chart3 { get; set; }
    public OklchColorValue? Chart4 { get; set; }
    public OklchColorValue? Chart5 { get; set; }
    public OklchColorValue? SidebarBackground { get; set; }
    public OklchColorValue? SidebarForeground { get; set; }
    public OklchColorValue? SidebarPrimary { get; set; }
    public OklchColorValue? SidebarPrimaryForeground { get; set; }
    public OklchColorValue? SidebarAccent { get; set; }
    public OklchColorValue? SidebarAccentForeground { get; set; }
    public OklchColorValue? SidebarBorder { get; set; }
    public OklchColorValue? SidebarRing { get; set; }
    public OklchColorValue? Shadow { get; set; }

    public ShadowValue? XXS { get; set; }
    public ShadowValue? XS { get; set; }
    public ShadowValue? SM { get; set; }
    public ShadowValue? Default { get; set; }
    public ShadowValue? MD { get; set; }
    public ShadowValue? LG { get; set; }
    public ShadowValue? XL { get; set; }
    public ShadowValue? XXL { get; set; }

    public ShadowValue? InsetXXS => XXS?.ToInset();
    public ShadowValue? InsetXS => XS?.ToInset();
    public ShadowValue? InsetSM => SM?.ToInset();
    public ShadowValue? Inset => Default?.ToInset();
    public ShadowValue? InsetMD => MD?.ToInset();
    public ShadowValue? InsetLG => LG?.ToInset();
    public ShadowValue? InsetXL => XL?.ToInset();
    public ShadowValue? InsetXXL => XXL?.ToInset();

    public NumberValue? Radius { get; set; }
    public NumberValue? Spacing { get; set; }

    public IEnumerator<string?> GetEnumerator()
    {
        yield return Background?.ToString(nameof(Background));
        yield return Foreground?.ToString(nameof(Foreground));
        yield return Card?.ToString(nameof(Card));
        yield return CardForeground?.ToString(nameof(CardForeground));
        yield return Popover?.ToString(nameof(Popover));
        yield return PopoverForeground?.ToString(nameof(PopoverForeground));
        yield return Primary?.ToString(nameof(Primary));
        yield return PrimaryForeground?.ToString(nameof(PrimaryForeground));
        yield return Secondary?.ToString(nameof(Secondary));
        yield return SecondaryForeground?.ToString(nameof(SecondaryForeground));
        yield return Muted?.ToString(nameof(Muted));
        yield return MutedForeground?.ToString(nameof(MutedForeground));
        yield return Accent?.ToString(nameof(Accent));
        yield return AccentForeground?.ToString(nameof(AccentForeground));
        yield return Destructive?.ToString(nameof(Destructive));
        yield return DestructiveForeground?.ToString(nameof(DestructiveForeground));
        yield return Border?.ToString(nameof(Border));
        yield return Input?.ToString(nameof(Input));
        yield return Ring?.ToString(nameof(Ring));
        yield return Chart1?.ToString(nameof(Chart1));
        yield return Chart2?.ToString(nameof(Chart2));
        yield return Chart3?.ToString(nameof(Chart3));
        yield return Chart4?.ToString(nameof(Chart4));
        yield return Chart5?.ToString(nameof(Chart5));
        yield return SidebarBackground?.ToString(nameof(SidebarBackground));
        yield return SidebarForeground?.ToString(nameof(SidebarForeground));
        yield return SidebarPrimary?.ToString(nameof(SidebarPrimary));
        yield return SidebarPrimaryForeground?.ToString(nameof(SidebarPrimaryForeground));
        yield return SidebarAccent?.ToString(nameof(SidebarAccent));
        yield return SidebarAccentForeground?.ToString(nameof(SidebarAccentForeground));
        yield return SidebarBorder?.ToString(nameof(SidebarBorder));
        yield return SidebarRing?.ToString(nameof(SidebarRing));

        yield return Shadow?.ToString(nameof(Shadow));

        yield return XXS?.ToString(nameof(XXS));
        yield return XS?.ToString(nameof(XS));
        yield return SM?.ToString(nameof(SM));
        yield return Default?.ToString(nameof(Default));
        yield return MD?.ToString(nameof(MD));
        yield return LG?.ToString(nameof(LG));
        yield return XL?.ToString(nameof(XL));
        yield return XXL?.ToString(nameof(XXL));
        yield return InsetXXS?.ToString(nameof(InsetXXS));
        yield return InsetXS?.ToString(nameof(InsetXS));
        yield return InsetSM?.ToString(nameof(InsetSM));
        yield return Inset?.ToString(nameof(Inset));
        yield return InsetMD?.ToString(nameof(InsetMD));
        yield return InsetLG?.ToString(nameof(InsetLG));
        yield return InsetXL?.ToString(nameof(InsetXL));
        yield return InsetXXL?.ToString(nameof(InsetXXL));

        yield return Radius?.ToString(nameof(Radius), nameof(Radius));
        yield return Spacing?.ToString(nameof(Spacing), nameof(Spacing));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string ToXaml(string otherXaml, bool appendAssembly)
    {
        var lineSb = new StringBuilder();

        foreach (var var in this)
        {
            if (string.IsNullOrWhiteSpace(var))
                continue;

            if (otherXaml.Contains(var))
                continue;

            lineSb.AppendLine($"    {var}");
        }

        var str = lineSb.ToString();

        var sb = new StringBuilder();
        sb.AppendLine("<ResourceDictionary");
        sb.AppendLine("    xmlns=\"https://github.com/avaloniaui\"");
        sb.AppendLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        if (str.Contains("values:"))
            sb.AppendLine($"    xmlns:values=\"clr-namespace:Manta.Themes.Values{(appendAssembly ? ";assembly=Manta" : "")}\"");
        if (str.Contains("system:"))
            sb.AppendLine("    xmlns:system=\"clr-namespace:System;assembly=System.Runtime\"");
        sb.AppendLine("    x:ClassModifier=\"internal\">");
        sb.Append(str);
        sb.AppendLine("</ResourceDictionary>");

        return sb.ToString();
    }
}
