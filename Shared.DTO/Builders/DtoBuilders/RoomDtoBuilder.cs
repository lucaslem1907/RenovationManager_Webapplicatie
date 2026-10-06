using Domain.Enums;
using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class RoomDtoBuilder
{
    private string _name = "Living Room";
    private RoomStatus _status = RoomStatus.not_started;

    public RoomDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RoomDtoBuilder WithStatus(RoomStatus status)
    {
        _status = status;
        return this;
    }

    public RoomDto Build()
    {
        return new RoomDto
        {
            Name = _name,
            Status = _status
        };
    }
}
