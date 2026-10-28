using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Infrastructure.Services;
using Shared.Builders.EntityBuilders;

namespace Reno.Test.Factories;

public static class TestJwtTokenFactory
{
    public const string TestJwtKey = "ThisIsATestJwtKey-AtLeast32CharsLong!";
    public static async Task<string> CreateTokenAsync(string role)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = TestJwtKey
            })
            .Build();

        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        userManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { role });

        // Arrange
        var jwtService = new JwtService(config, userManagerMock.Object);
        var password= new PasswordService().HashPassword("TestPassword123!");

        var user = new UserBuilder().WithLogin("testuser")
            .WithFirstName("Test")
            .WithLastName("User")
            .WithPasswordHash(password)
            .Build();

        return await jwtService.GenerateToken(user);
    }
}
