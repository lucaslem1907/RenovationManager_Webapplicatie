using System.Net;

namespace Domain.Entities;

public class Subtask
{
    public Guid Id { get; set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }

    public Guid TaskItemId { get; private set; }

    public TaskItem TaskItem { get; private set; } = null!;

    private Subtask() { } // EF Core

    public Subtask(string title, Guid taskItemId, bool status)
    {
        Id = Guid.NewGuid();
        Title = title;
        TaskItemId = taskItemId;
        IsCompleted = status;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public void UpdateSubtask(string title, bool status)
    {
        Title = title;
        IsCompleted = status;
    }
}