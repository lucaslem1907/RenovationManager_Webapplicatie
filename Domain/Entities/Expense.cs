namespace Domain.Entities;

public class Expense 
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public decimal Amount { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;


    private Expense() { }

    public Expense(decimal amount, string name, Guid projectId)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Name = name;
        ProjectId = projectId;
    }
}