namespace Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }

    public ExpenseStatus Status { get; set; }

    public DateTime CreatedDate { get; set; }


    //references
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    public Guid? RoomId { get; set; }
    public Room? Room { get; private set; }

    private Expense() { }

    public Expense(decimal amount, string name, Guid projectId, Guid? roomId, string description, ExpenseStatus status)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Name = name;
        Description = description;
        ProjectId = projectId;
        RoomId = roomId;
        Status = status;
        CreatedDate = DateTime.UtcNow;
    }
}