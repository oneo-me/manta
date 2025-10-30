using System.Globalization;
using LightKit.Globalization.Core;
using LightKit.Reactive.Views;

namespace LightKit.Globalization;

sealed class LangSource : Model
{
    public LangType Current
    {
        get;
        set
        {
            if (SetValue(ref field, value) is false)
                return;

            var cultureInfo = value.ToCultureInfo();
            if (cultureInfo is null)
                return;

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
