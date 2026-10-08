using Domain.Entities;

namespace Shared.Builders.EntityBuilders;

public class SubtaskBuilder
{
    private string _title = "Default subtask";
    private Guid _taskItemId = Guid.NewGuid();
    private bool _isCompleted;

    public SubtaskBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public SubtaskBuilder WithTaskItemId(Guid taskItemId)
    {
        _taskItemId = taskItemId;
        return this;
    }

    public SubtaskBuilder WithIsCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        return this;
    }

    public Subtask Build()
    {
        return new Subtask(_title, _taskItemId, _isCompleted);
    }
}
