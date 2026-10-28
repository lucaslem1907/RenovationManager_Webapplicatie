using Reno.Test.Factories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Reno.Test.Integration;

public class JwtServiceIntegrationTests
{
    [Fact]
    public async Task CreateTokenAsync_ShouldIncludeRoleClaim()
    {
        // Arrange
        var token = await TestJwtTokenFactory.CreateTokenAsync("Admin");

        // Act
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        // Assert
        Assert.NotNull(roleClaim);
        Assert.Equal("Admin", roleClaim!.Value);
    }
}
