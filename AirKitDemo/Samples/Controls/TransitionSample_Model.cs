using AirKit;
using AirKit.Attributes;
using AirKitDemo.Models;

namespace AirKitDemo.Samples.Controls;

[Sample(SampleGroupType.Controls, "Transition")]
[View<TransitionSample>]
sealed class TransitionSample_Model : ViewModel
{
    public string Content
    {
        get;
        set => SetValue(ref field, value);
    } = "Content 1";

    public void SetContent(string content)
    {
        Content = content;
    }
}
