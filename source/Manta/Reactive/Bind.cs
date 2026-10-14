using System.Globalization;
using System.Linq.Expressions;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Manta.Reactive;

public static class Bind
{
    public static BindingBase Create<TIn, TOut>
    (
        TIn source,
        Expression<Func<TIn, TOut>> expression,
        IValueConverter? converter = null,
        BindingMode mode = BindingMode.Default,
        BindingPriority priority = BindingPriority.LocalValue,
        CultureInfo? converterCulture = null,
        object? converterParameter = null,
        object? fallbackValue = null,
        string? stringFormat = null,
        object? targetNullValue = null,
        UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default,
        int delay = 0
    )
    {
        return CompiledBinding.Create(expression, source, converter, mode, priority, converterCulture, converterParameter, fallbackValue, stringFormat, targetNullValue, updateSourceTrigger, delay);
    }
}
