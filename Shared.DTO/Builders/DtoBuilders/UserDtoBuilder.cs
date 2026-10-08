using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class UserDtoBuilder
{
    private Guid _id = Guid.NewGuid();

    public UserDtoBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserDTO Build()
    {
        return new UserDTO
        {
            Id = _id
        };
    }
}
