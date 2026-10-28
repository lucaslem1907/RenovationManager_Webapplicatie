using Domain.Entities;

namespace Shared.Builders.EntityBuilders;

public class UserBuilder
{
    private string _firstName = "Lucas";
    private string _lastName = "Tester";
    private string _email = "lucas.tester@test.com";
    private string _passwordHash = "hash";
    private string _login = "tester";

    public UserBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithLogin(string login)
    {
        _login = login;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public User Build()
    {
        return new User(_firstName, _lastName, _login, _email, _passwordHash);
    }
}
