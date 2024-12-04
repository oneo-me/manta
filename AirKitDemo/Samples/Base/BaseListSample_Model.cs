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

sealed class BaseTreeData
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
    public List<BaseTreeData> Children { get; set; } = [];
}

[Sample(SampleGroupType.BaseControls, "List")]
[View<BaseListSample>]
sealed class BaseListSample_Model : ViewModel
{
    public List<BaseListData> Items5 { get; set; }
    public List<BaseListData> Items100 { get; set; }
    public List<BaseTreeData> TreeItems30 { get; set; }

    public BaseListSample_Model()
    {
        var faker = new Faker<BaseListData>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        Items5 = faker.Generate(5);
        Items100 = faker.Generate(100);

        var treeFaker = new Faker<BaseTreeData>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        TreeItems30 = GenerateNode(treeFaker, 30);
    }

    static List<BaseTreeData> GenerateNode(Faker<BaseTreeData> faker, int count, int depth = 0)
    {
        if (depth > 3)
            return [];

        var items = faker.Generate(count);

        foreach (var item in items)
            item.Children = GenerateNode(faker, count / 2, depth + 1);

        return items;
    }
}
