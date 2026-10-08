using Shared.DTO;

namespace Shared.Builders.DtoBuilders;

public class UserRegisterDtoBuilder
{
    private string _firstName = "Lucas";
    private string _lastName = "Tester";
    private string _email = "lucas.tester@test.com";
    private string _password = "Password123";

    public UserRegisterDtoBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserRegisterDtoBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserRegisterDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserRegisterDtoBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserRegisterDto Build()
    {
        return new UserRegisterDto
        {
            FirstName = _firstName,
            LastName = _lastName,
            Email = _email,
            Password = _password
        };
    }
}
