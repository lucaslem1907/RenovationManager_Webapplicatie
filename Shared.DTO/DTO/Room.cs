using Domain.Enums;

namespace Shared.DTO
{
    public class RoomDto
    {
        public string Name { get; set; } = string.Empty;
        public RoomStatus Status { get; set; }
    }
}
