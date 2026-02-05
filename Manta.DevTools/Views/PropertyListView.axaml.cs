using System.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Manta.Controls;
using Manta.DevTools.Models.Items;
using Manta.Globalization.Markups;
using Manta.Reactive.Views;

namespace Manta.DevTools.Views;

sealed partial class PropertyListView : MView
{
    /// <summary>
    /// Static template selector instance for XAML reference.
    /// </summary>
    public static PropertyItemTemplateSelector PropertyItemTemplate { get; } = new();

    public PropertyListView()
    {
        InitializeComponent();
    }
}

/// <summary>
/// Custom template selector for PropertyItemBase types.
/// </summary>
sealed class PropertyItemTemplateSelector : IDataTemplate
{
    public bool Match(object? data) => data is PropertyItemBase;

    public Control Build(object? param) => param switch
    {
        BooleanPropertyItem item => CreateBooleanEditor(item),
        StringPropertyItem item => CreateStringEditor(item),
        NumericPropertyItem item => CreateNumericEditor(item),
        EnumPropertyItem item => CreateEnumEditor(item),
        CommandPropertyItem item => CreateCommandEditor(item),
        CollectionPropertyItem item => CreateCollectionButton(item),
        ObjectPropertyItem item => CreateObjectButton(item),
        ReadOnlyPropertyItem item => CreateReadOnlyDisplay(item),
        PropertyItemBase item => CreateFallbackDisplay(item),
        _ => new TextBlock { Text = "Unknown" }
    };

    static Control CreateBooleanEditor(BooleanPropertyItem item)
    {
        var checkBox = new MCheckBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new(8, 0, 0, 0),
            IsEnabled = !item.IsReadOnly
        };

        var bindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
        checkBox[!ToggleButton.IsCheckedProperty] = new Binding(bindingPath, BindingMode.TwoWay) { Source = item.Source };

        return checkBox;
    }

    static Control CreateStringEditor(StringPropertyItem item)
    {
        if (item.IsReadOnly)
        {
            var textBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new(8, 0, 0, 0)
            };

            var bindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
            textBlock[!TextBlock.TextProperty] = new LangExtension(new Binding(bindingPath) { Source = item.Source }).ToBinding();

            return textBlock;
        }

        var textBox = new MTextBox
        {
            Padding = new(5),
            Margin = new(4)
        };

        var editBindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
        textBox[!TextBox.TextProperty] = new LangExtension(new Binding(editBindingPath, BindingMode.TwoWay) { Source = item.Source }).ToBinding();

        return textBox;
    }

    static Control CreateNumericEditor(NumericPropertyItem item)
    {
        if (item.IsReadOnly)
        {
            var textBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new(8, 0, 0, 0)
            };

            var bindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
            textBlock[!TextBlock.TextProperty] = new Binding(bindingPath) { Source = item.Source };

            return textBlock;
        }

        var textBox = new MTextBox
        {
            Padding = new(5),
            Margin = new(4)
        };

        var editBindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
        textBox[!TextBox.TextProperty] = new Binding(editBindingPath, BindingMode.TwoWay) { Source = item.Source };

        return textBox;
    }

    static Control CreateEnumEditor(EnumPropertyItem item)
    {
        if (item.IsReadOnly)
        {
            var textBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new(8, 0, 0, 0)
            };

            var bindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
            textBlock[!TextBlock.TextProperty] = new Binding(bindingPath) { Source = item.Source };

            return textBlock;
        }

        var comboBox = new MComboBox
        {
            ItemsSource = item.EnumValues,
            Padding = new(5),
            Margin = new(4)
        };

        var editBindingPath = item.IsReactive ? $"{item.Name}.Value" : item.Name;
        comboBox[!SelectingItemsControl.SelectedItemProperty] = new Binding(editBindingPath, BindingMode.TwoWay) { Source = item.Source };

        return comboBox;
    }

    static Control CreateCommandEditor(CommandPropertyItem item)
    {
        var button = new MButton
        {
            Padding = new(6),
            HorizontalAlignment = HorizontalAlignment.Right,
            Variant = MButtonVariant.Outline,
            Content = new MIcon
            {
                IconSize = 14,
                IconSource = Icons.Play,
                VerticalAlignment = VerticalAlignment.Center
            },
            Command = item.Command,
            IsEnabled = !item.HasParameter
        };

        if (item.HasParameter)
        {
            ToolTip.SetTip(button, "Command requires parameter");
        }

        return button;
    }

    static Control CreateCollectionButton(CollectionPropertyItem item)
    {
        var button = new MButton
        {
            Variant = MButtonVariant.Ghost,
            Padding = new(6),
            Margin = new(4),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Left
        };

        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4
        };

        stackPanel.Children.Add(new TextBlock
        {
            Text = item.PropertyType.Name,
            Foreground = Avalonia.Media.Brushes.Gray
        });

        stackPanel.Children.Add(new TextBlock
        {
            Text = $"[{item.Count}]",
            Foreground = Avalonia.Media.Brushes.DodgerBlue
        });

        button.Content = stackPanel;
        button.Click += async (sender, _) =>
        {
            if (item.Value is not IEnumerable enumerable)
                return;

            var dialog = new CollectionPropertyDialog_Model
            {
                PropertyName = item.Name,
                PropertyValue = enumerable
            };

            var parentView = (sender as Control)?.FindAncestorOfType<MView>();
            if (parentView?.DataContext is ViewModel parentViewModel)
                await dialog.ShowDialogAsync(parentViewModel);
        };

        return button;
    }

    static Control CreateObjectButton(ObjectPropertyItem item)
    {
        var button = new MButton
        {
            Variant = MButtonVariant.Ghost,
            Padding = new(6),
            Margin = new(4),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Content = new TextBlock
            {
                Text = item.PropertyType.Name,
                Foreground = Avalonia.Media.Brushes.Gray
            }
        };

        button.Click += async (sender, _) =>
        {
            var dialog = new ObjectPropertyDialog_Model
            {
                PropertyName = item.Name,
                PropertyValue = item.Value
            };

            var parentView = (sender as Control)?.FindAncestorOfType<MView>();
            if (parentView?.DataContext is ViewModel parentViewModel)
                await dialog.ShowDialogAsync(parentViewModel);
        };

        return button;
    }

    static Control CreateReadOnlyDisplay(ReadOnlyPropertyItem item)
    {
        return new TextBlock
        {
            Text = item.DisplayValue,
            VerticalAlignment = VerticalAlignment.Center,
            Padding = new(8, 0, 0, 0),
            Foreground = Avalonia.Media.Brushes.Gray
        };
    }

    static Control CreateFallbackDisplay(PropertyItemBase item)
    {
        return new TextBlock
        {
            Text = $"[{item.GetType().Name}]",
            VerticalAlignment = VerticalAlignment.Center,
            Padding = new(8, 0, 0, 0),
            Foreground = Avalonia.Media.Brushes.Orange
        };
    }
}
