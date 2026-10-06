using Shared.DTO;

namespace Shared.Tests.DTOBuilders;

public class ProjectDtoBuilder
{
    private string _name = "New Kitchen";
    private string _description = "Fixing the kitchen";
    private string _address = "Street 1";
    private decimal _budget = 10_000;
    private DateTime _startDate = DateTime.UtcNow;
    private Guid _ownerId = Guid.NewGuid();

    public ProjectDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectDtoBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public ProjectDtoBuilder WithBudget(decimal budget)
    {
        _budget = budget;
        return this;
    }

    public ProjectDtoBuilder WithStartDate(DateTime startDate)
    {
        _startDate = startDate;
        return this;
    }

    public ProjectDtoBuilder WithOwnerId(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public ProjectDto Build()
    {
        return new ProjectDto
        {
            Name = _name,
            Description = _description,
            Address = _address,
            Budget = _budget,
            StartDate = _startDate,
            OwnerId = _ownerId
        };
    }
}
