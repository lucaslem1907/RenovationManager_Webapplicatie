namespace Domain.Entities;

public class Subtask
{
    public Guid Id { get; set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }

    public Guid TaskItemId { get; private set; }

    public TaskItem TaskItem { get; private set; } = null!;

    private Subtask() { } // EF Core

    internal Subtask(string title, Guid taskItemId)
    {
        Id = Guid.NewGuid();
        Title = title;
        TaskItemId = taskItemId;
        IsCompleted = false;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}