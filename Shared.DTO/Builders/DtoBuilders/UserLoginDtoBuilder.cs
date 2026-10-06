using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class UserLoginDtoBuilder
{
    private string _login = "ltester";
    private string _password = "Password123";

    public UserLoginDtoBuilder WithLogin(string login)
    {
        _login = login;
        return this;
    }

    public UserLoginDtoBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserLoginDto Build()
    {
        return new UserLoginDto
        {
            login = _login,
            password = _password
        };
    }
}
