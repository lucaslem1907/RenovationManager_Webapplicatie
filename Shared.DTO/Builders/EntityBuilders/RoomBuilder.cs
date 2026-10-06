using Domain.Entities;
using Domain.Enums;

namespace Shared.Builders.EntityBuilders;

public class RoomBuilder
{
    private string _name = "Living Room";
    private RoomStatus _status = RoomStatus.not_started;
    private Guid _projectId = Guid.NewGuid();

    public RoomBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RoomBuilder WithStatus(RoomStatus status)
    {
        _status = status;
        return this;
    }

    public RoomBuilder WithProjectId(Guid projectId)
    {
        _projectId = projectId;
        return this;
    }

    public Room Build()
    {
        var room = new Room(_name, _status)
        {
            ProjectId = _projectId
        };

        return room;
    }
}
