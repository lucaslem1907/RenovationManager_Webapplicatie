using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class TaskDtoBuilder
{
    private string _title = "Default task";
    private string _description = "";
    private bool _isCompleted;

    public TaskDtoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public TaskDtoBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TaskDtoBuilder WithIsCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        return this;
    }

    public TaskDto Build()
    {
        return new TaskDto
        {
            Title = _title,
            Description = _description,
            IsCompleted = _isCompleted
        };
    }
}
