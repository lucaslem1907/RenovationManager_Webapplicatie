using Reno.Test.Factories;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Reno.Test.Integration;

public class AdministrationIntegrationTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public AdministrationIntegrationTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AssignRole_WhenCallerIsNotAdmin_ShouldReturnForbidden()
    {
        // Arrange
        var token = await TestJwtTokenFactory.CreateTokenAsync("User");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/Administration/assign-role?userId=test-user-id&roleName=User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AssignRole_WhenCallerIsAdmin_ShouldReturnOk()
    {
        // Arrange
        var token = await TestJwtTokenFactory.CreateTokenAsync("Admin");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/Administration/assign-role?userId=test-user-id&roleName=User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact] 
    public async Task DeleteRole_WhenCallerIsNotAdmin_ShouldReturnForbidden()
    {
        // Arrange
        var token = await TestJwtTokenFactory.CreateTokenAsync("User");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/Administration/delete-role?userId=test-user-id&roleName=User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // Act
        var response = await _client.SendAsync(request);
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeleteRole_WhenCallerIsAdmin_ShouldReturnOk()
    {
        // Arrange
        var token = await TestJwtTokenFactory.CreateTokenAsync("Admin");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/Administration/delete-role?userId=test-user-id&roleName=User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // Act
        var response = await _client.SendAsync(request);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }   
}
