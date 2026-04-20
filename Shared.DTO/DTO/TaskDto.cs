namespace Shared.DTO
{
    public class TaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
