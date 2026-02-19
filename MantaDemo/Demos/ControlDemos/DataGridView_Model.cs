using System.Collections.ObjectModel;
using Manta.DataGrid.Controls;
using Manta.Reactive.Commands;
using Manta.Reactive.Views;
using MantaDemo.Reactive;

namespace MantaDemo.Demos.ControlDemos;

sealed class UserItem(string name, string email, int age, bool active)
{
    public string Name { get; } = name;
    public string Email { get; } = email;
    public int Age { get; } = age;
    public bool Active { get; } = active;
    public bool Inactive { get; } = !active;
}

[View<DataGridView>]
sealed class DataGridView_Model : DemoViewModel
{
    public static DataGridView_Model Design => new();

    public override DemoGroup Group => DemoGroup.Controls;
    public override DemoSubGroup SubGroup => DemoSubGroup.Advanced;
    public override string Header => nameof(MDataGrid);

    public ObservableCollection<UserItem> Users { get; }

    UserItem? _selectedUser;
    public UserItem? SelectedUser
    {
        get => _selectedUser;
        set => SetValue(ref _selectedUser, value);
    }

    public CommandWarper AddRowCommand { get; }
    public CommandWarper RemoveRowCommand { get; }

    private int _counter = 10;

    public DataGridView_Model()
    {
        Users =
        [
            new("Alice Johnson",  "alice@example.com",  28, true),
            new("Bob Smith",      "bob@example.com",    34, false),
            new("Carol White",    "carol@example.com",  22, true),
            new("David Brown",    "david@example.com",  41, true),
            new("Eva Martinez",   "eva@example.com",    30, false),
            new("Frank Lee",      "frank@example.com",  27, true),
            new("Grace Kim",      "grace@example.com",  35, true),
            new("Henry Zhang",    "henry@example.com",  29, false),
            new("Iris Patel",     "iris@example.com",   26, true),
            new("Jack Wilson",    "jack@example.com",   38, true),
        ];

        AddRowCommand = new(() =>
        {
            _counter++;
            Users.Add(new($"User {_counter}", $"user{_counter}@example.com", 20 + _counter % 40, _counter % 2 == 0));
        });

        RemoveRowCommand = new(() =>
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
            }
            else if (Users.Count > 0)
            {
                Users.RemoveAt(Users.Count - 1);
            }
        });
    }
}
