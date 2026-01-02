using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Manta.Tools.Values;

namespace Manta.Tools.Themes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class ThemeVariable : IEnumerable<string>
{
    public OklchColorValue Background { get; set; } = new();
    public OklchColorValue Foreground { get; set; } = new();
    public OklchColorValue Card { get; set; } = new();
    public OklchColorValue CardForeground { get; set; } = new();
    public OklchColorValue Popover { get; set; } = new();
    public OklchColorValue PopoverForeground { get; set; } = new();
    public OklchColorValue Primary { get; set; } = new();
    public OklchColorValue PrimaryForeground { get; set; } = new();
    public OklchColorValue Secondary { get; set; } = new();
    public OklchColorValue SecondaryForeground { get; set; } = new();
    public OklchColorValue Muted { get; set; } = new();
    public OklchColorValue MutedForeground { get; set; } = new();
    public OklchColorValue Accent { get; set; } = new();
    public OklchColorValue AccentForeground { get; set; } = new();
    public OklchColorValue Destructive { get; set; } = new();
    public OklchColorValue DestructiveForeground { get; set; } = new();
    public OklchColorValue Border { get; set; } = new();
    public OklchColorValue Input { get; set; } = new();
    public OklchColorValue Ring { get; set; } = new();
    public OklchColorValue Chart1 { get; set; } = new();
    public OklchColorValue Chart2 { get; set; } = new();
    public OklchColorValue Chart3 { get; set; } = new();
    public OklchColorValue Chart4 { get; set; } = new();
    public OklchColorValue Chart5 { get; set; } = new();
    public OklchColorValue SidebarBackground { get; set; } = new();
    public OklchColorValue SidebarForeground { get; set; } = new();
    public OklchColorValue SidebarPrimary { get; set; } = new();
    public OklchColorValue SidebarPrimaryForeground { get; set; } = new();
    public OklchColorValue SidebarAccent { get; set; } = new();
    public OklchColorValue SidebarAccentForeground { get; set; } = new();
    public OklchColorValue SidebarBorder { get; set; } = new();
    public OklchColorValue SidebarRing { get; set; } = new();
    public OklchColorValue Shadow { get; set; } = new();

    public ShadowValue XXS { get; set; } = new();
    public ShadowValue XS { get; set; } = new();
    public ShadowValue SM { get; set; } = new();
    public ShadowValue Default { get; set; } = new();
    public ShadowValue MD { get; set; } = new();
    public ShadowValue LG { get; set; } = new();
    public ShadowValue XL { get; set; } = new();
    public ShadowValue XXL { get; set; } = new();

    public ShadowValue InsetXXS => XXS.ToInset();
    public ShadowValue InsetXS => XS.ToInset();
    public ShadowValue InsetSM => SM.ToInset();
    public ShadowValue Inset => Default.ToInset();
    public ShadowValue InsetMD => MD.ToInset();
    public ShadowValue InsetLG => LG.ToInset();
    public ShadowValue InsetXL => XL.ToInset();
    public ShadowValue InsetXXL => XXL.ToInset();

    public NumberValue Radius { get; set; } = new();
    public NumberValue Spacing { get; set; } = new();

    public IEnumerator<string> GetEnumerator()
    {
        yield return Background.ToString(nameof(Background));
        yield return Foreground.ToString(nameof(Foreground));
        yield return Card.ToString(nameof(Card));
        yield return CardForeground.ToString(nameof(CardForeground));
        yield return Popover.ToString(nameof(Popover));
        yield return PopoverForeground.ToString(nameof(PopoverForeground));
        yield return Primary.ToString(nameof(Primary));
        yield return PrimaryForeground.ToString(nameof(PrimaryForeground));
        yield return Secondary.ToString(nameof(Secondary));
        yield return SecondaryForeground.ToString(nameof(SecondaryForeground));
        yield return Muted.ToString(nameof(Muted));
        yield return MutedForeground.ToString(nameof(MutedForeground));
        yield return Accent.ToString(nameof(Accent));
        yield return AccentForeground.ToString(nameof(AccentForeground));
        yield return Destructive.ToString(nameof(Destructive));
        yield return DestructiveForeground.ToString(nameof(DestructiveForeground));
        yield return Border.ToString(nameof(Border));
        yield return Input.ToString(nameof(Input));
        yield return Ring.ToString(nameof(Ring));
        yield return Chart1.ToString(nameof(Chart1));
        yield return Chart2.ToString(nameof(Chart2));
        yield return Chart3.ToString(nameof(Chart3));
        yield return Chart4.ToString(nameof(Chart4));
        yield return Chart5.ToString(nameof(Chart5));
        yield return SidebarBackground.ToString(nameof(SidebarBackground));
        yield return SidebarForeground.ToString(nameof(SidebarForeground));
        yield return SidebarPrimary.ToString(nameof(SidebarPrimary));
        yield return SidebarPrimaryForeground.ToString(nameof(SidebarPrimaryForeground));
        yield return SidebarAccent.ToString(nameof(SidebarAccent));
        yield return SidebarAccentForeground.ToString(nameof(SidebarAccentForeground));
        yield return SidebarBorder.ToString(nameof(SidebarBorder));
        yield return SidebarRing.ToString(nameof(SidebarRing));

        yield return Shadow.ToString(nameof(Shadow));

        yield return XXS.ToString(nameof(XXS));
        yield return XS.ToString(nameof(XS));
        yield return SM.ToString(nameof(SM));
        yield return Default.ToString(nameof(Default));
        yield return MD.ToString(nameof(MD));
        yield return LG.ToString(nameof(LG));
        yield return XL.ToString(nameof(XL));
        yield return XXL.ToString(nameof(XXL));
        yield return InsetXXS.ToString(nameof(InsetXXS));
        yield return InsetXS.ToString(nameof(InsetXS));
        yield return InsetSM.ToString(nameof(InsetSM));
        yield return Inset.ToString(nameof(Inset));
        yield return InsetMD.ToString(nameof(InsetMD));
        yield return InsetLG.ToString(nameof(InsetLG));
        yield return InsetXL.ToString(nameof(InsetXL));
        yield return InsetXXL.ToString(nameof(InsetXXL));

        yield return Radius.ToString(nameof(Radius), nameof(Radius));
        yield return Spacing.ToString(nameof(Spacing), nameof(Spacing));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string ToXaml()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<ResourceDictionary");
        sb.AppendLine("    xmlns=\"https://github.com/avaloniaui\"");
        sb.AppendLine("    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        sb.AppendLine("    xmlns:values=\"clr-namespace:Manta.Themes.Values\"");
        sb.AppendLine("    xmlns:system=\"clr-namespace:System;assembly=System.Runtime\"");
        sb.AppendLine("    x:ClassModifier=\"internal\">");

        foreach (var var in this)
            sb.AppendLine($"    {var}");

        sb.AppendLine("</ResourceDictionary>");

        return sb.ToString();
    }
}
