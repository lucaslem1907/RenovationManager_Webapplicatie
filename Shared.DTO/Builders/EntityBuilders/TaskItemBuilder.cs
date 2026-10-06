using Domain.Entities;

namespace Shared.Builders.EntityBuilders;

public class TaskItemBuilder
{
    private string _title = "Default task";
    private string _description = "";
    private bool _isCompleted;
    private Guid _roomId = Guid.NewGuid();

    public TaskItemBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public TaskItemBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TaskItemBuilder WithIsCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        return this;
    }

    public TaskItemBuilder WithRoomId(Guid roomId)
    {
        _roomId = roomId;
        return this;
    }

    public TaskItem Build()
    {
        var task = new TaskItem(_title, _roomId);
        task.UpdateTask(_title, _description, _isCompleted);

        return task;
    }
}
