namespace Reno.DTO
{
    public class ProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }
        public Guid OwnerId { get; set; }
    }
}
