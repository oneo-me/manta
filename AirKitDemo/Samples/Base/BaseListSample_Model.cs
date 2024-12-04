using AirKit;
using AirKit.Attributes;
using AirKitDemo.Models;
using Bogus;

namespace AirKitDemo.Samples.Base;

sealed class BaseListData
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
}

[Sample(SampleGroupType.BaseControls, "List")]
[View<BaseListSample>]
sealed class BaseListSample_Model : ViewModel
{
    public List<BaseListData> Items5 { get; set; }
    public List<BaseListData> Items100 { get; set; }

    public BaseListSample_Model()
    {
        var faker = new Faker<BaseListData>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        Items5 = faker.Generate(5);
        Items100 = faker.Generate(100);
    }
}
