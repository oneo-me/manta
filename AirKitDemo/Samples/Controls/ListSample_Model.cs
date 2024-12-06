using AirKit;
using AirKit.Attributes;
using AirKitDemo.Models;
using Bogus;

namespace AirKitDemo.Samples.Controls;

sealed class ListData
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
}

sealed class TreeData
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
    public List<TreeData> Children { get; set; } = [];
}

[Sample(SampleGroupType.Controls, "List")]
[View<ListSample>]
sealed class ListSample_Model : ViewModel
{
    public List<ListData> Items5 { get; set; }
    public List<ListData> Items100 { get; set; }
    public List<TreeData> TreeItems30 { get; set; }

    public ListSample_Model()
    {
        var faker = new Faker<ListData>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        Items5 = faker.Generate(5);
        Items100 = faker.Generate(100);

        var treeFaker = new Faker<TreeData>()
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        TreeItems30 = GenerateNode(treeFaker, 30);
    }

    static List<TreeData> GenerateNode(Faker<TreeData> faker, int count, int depth = 0)
    {
        if (depth > 3)
            return [];

        var items = faker.Generate(count);

        foreach (var item in items)
            item.Children = GenerateNode(faker, count / 2, depth + 1);

        return items;
    }
}
