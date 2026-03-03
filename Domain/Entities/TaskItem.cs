using System.Text.Json.Serialization;

namespace Domain.Entities;

public class TaskItem 
{
    public Guid Id { get; set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public Guid RoomId { get; set; }

    [JsonIgnore]
    public Room Room { get;  set; } = null!;


    private readonly List<Subtask> _subtasks = new();

    public ICollection<Subtask> Subtasks => _subtasks;

    public TaskItem() { }

    public TaskItem(string title, string description, Guid roomId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        RoomId = roomId;

    }
}