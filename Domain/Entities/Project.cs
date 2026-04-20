namespace Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public string? Address { get; private set; }

    public decimal? Budget { get; private set; }

    public DateTime StartDate { get; private set; } = DateTime.UtcNow;

    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; }

    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    public ICollection<Expense> Expenses { get; private set; } = new List<Expense>();

    private Project() { }

    public Project(string name, User ownerId, string? address, string description = "")
    {
        Id = Guid.NewGuid();
        Address = address;
        Name = name;
        Owner = ownerId;
        Description = description;
    }

    public void UpdateProject(string name, string description, string address, decimal? budget, DateTime startDate)
    {
        Name = name;
        Description = description;
        Address = address;
        Budget = budget;
        StartDate = startDate;
    }

}