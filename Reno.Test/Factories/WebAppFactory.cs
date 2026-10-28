using Application.Administration;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shared.Builders.EntityBuilders;

namespace Reno.Test.Factories;

public class WebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = TestJwtTokenFactory.TestJwtKey,
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=test;Username=test;Password=test"
            });
        });

        builder.ConfigureServices(services =>
        {
            UseInMemoryDatabase(services);
            ReplaceWithMocks(services);
        });
    }

    /// <summary>
    /// Make the database in memory for testing.
    /// </summary>
    private static void UseInMemoryDatabase(IServiceCollection services)
    {
        services.RemoveAll<DbContextOptions<DatabaseContext>>();

        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        services.AddSingleton(connection);

        services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connection));
    }

    // Give the database a mock user repository, user manager, and role manager
    // Why? Because we don't want to hit the database for these things, we just want to test the controller logic.
    // How? By removing the existing services and adding our own mocks.
    private static void ReplaceWithMocks(IServiceCollection services)
    {
        services.RemoveAll<IUserRepository>();
        services.RemoveAll<UserManager<User>>();
        services.RemoveAll<RoleManager<IdentityRole>>();
        services.RemoveAll<AdministrationUsecase>();

        services.AddSingleton(CreateUserRepositoryMock());
        services.AddSingleton(CreateUserManagerMock());
        services.AddSingleton(CreateRoleManagerMock());
        services.AddScoped<AdministrationUsecase>();
    }

    /// Create a mock user repository that returns a specific user when GetById is called.
    private static IUserRepository CreateUserRepositoryMock()
    {
        var targetUser = new UserBuilder().WithFirstName("test")
            .WithLastName("user")
            .WithLogin("testuser")
            .WithPasswordHash("hashedpassword")
            .Build();

        var mock = new Mock<IUserRepository>();
        mock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(targetUser);
        mock.Setup(x => x.SaveChanges()).Returns(Task.CompletedTask);
        return mock.Object;
    }

    /// Create a mock user manager that returns success when AddToRoleAsync is called.
    private static UserManager<User> CreateUserManagerMock()
    {
        var mock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        mock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        return mock.Object;
    }

    /// Create a mock role manager that returns true when RoleExistsAsync is called.
    private static RoleManager<IdentityRole> CreateRoleManagerMock()
    {
        var mock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

        mock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        return mock.Object;
    }
}