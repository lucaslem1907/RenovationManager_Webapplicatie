using System.Xml.Linq;

namespace Domain.Entities;

public class Project 
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Guid OwnerId { get; private set; }

    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    public ICollection<Expense> Expenses { get; private set; } = new List<Expense>();

    private Project() { }

    public Project(string name, Guid ownerId, string description = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        OwnerId = ownerId;
        Description = description;
    }

}