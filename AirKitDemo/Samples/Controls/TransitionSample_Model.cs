using AirKitDemo.Langs;
using AirKitDemo.Models;
using Orphic;

namespace AirKitDemo.Samples.Controls;

[Sample(SampleGroupType.Controls, LangKey.SampleName_Transition)]
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
