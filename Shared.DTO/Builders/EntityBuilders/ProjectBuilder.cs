using Domain.Entities;

namespace Shared.Builders.EntityBuilders;

public class ProjectBuilder
{
    private string _name = "Test Project";
    private string _address = "Street 1";
    private string _description = "Project description";
    private decimal _budget = 100;
    private User _owner = new UserBuilder().Build();

    public ProjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public ProjectBuilder WithBudget(decimal budget)
    {
        _budget = budget;
        return this;
    }

    public ProjectBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectBuilder WithOwner(User owner)
    {
        _owner = owner;
        return this;
    }

    public Project Build()
    {
        return new Project(_name, _owner, _address, _description, _budget);
    }
}
