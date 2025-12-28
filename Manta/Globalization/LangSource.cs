using System.Globalization;
using Manta.Globalization.Core;
using Manta.Reactive.Views;

namespace Manta.Globalization;

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
