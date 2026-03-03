using System.Text.Json.Serialization;
using Domain.Enums;
namespace Domain.Entities;

public class Room 
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public RoomStatus Status { get; set; }

    public Guid ProjectId { get; set; }

    [JsonIgnore]
    public Project Project { get; set; } = null!;

    public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    public Room() { }

    public Room(string name, RoomStatus status)
    {
        Id = Guid.NewGuid();
        Name = name;
        Status = status;
    }

}