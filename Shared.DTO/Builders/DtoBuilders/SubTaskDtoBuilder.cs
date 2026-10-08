using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class SubTaskDtoBuilder
{
    private string _title = "Default subtask";
    private bool _isCompleted;

    public SubTaskDtoBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public SubTaskDtoBuilder WithIsCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        return this;
    }

    public SubTaskDto Build()
    {
        return new SubTaskDto
        {
            Title = _title,
            IsCompleted = _isCompleted
        };
    }
}
