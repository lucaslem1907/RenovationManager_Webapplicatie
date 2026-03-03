using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Room 
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid ProjectId { get; set; }

    [JsonIgnore]
    public Project Project { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    public Room() { }

    public Room(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

}