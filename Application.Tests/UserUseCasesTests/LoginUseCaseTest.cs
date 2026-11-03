using Application.Interfaces;
using Application.Users;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Entities;
using Moq;
using Shared.DTO;
using Shared.Builders.EntityBuilders;
using Shared.Builders.EntityBuilders;
using Shared.Builders.DtoBuilders;

namespace Application.Tests.UserUseCasesTests
{
    public class LoginUseCaseTest
    {

        [Fact]
        public void LoginUseCase_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUseCase(mockRepo.Object, mockAuthService.Object);

            // Act
            var User = new UserBuilder()
                .WithLogin("testuser")
                .WithPasswordHash("ValidPasswordHash")
                .Build();
            var userLoginDto = new UserLoginDtoBuilder()
                .WithLogin("testuser")
                .WithPassword("ValidPasswordHash")
                .Build();

            // Assert
            mockRepo.Setup(repo => repo.GetByLogin("testuser")).ReturnsAsync(User);
            mockAuthService.Setup(auth => auth.VerifyPassword(userLoginDto.password, User.PasswordHash)).Returns(true);
            mockAuthService.Setup(auth => auth.GenerateToken(User)).ReturnsAsync("token");

            var result = useCase.Execute(userLoginDto).Result;

            Assert.True(result.succes);
            Assert.Equal("token", result.token);
        }

        [Fact]
        public void LoginUseCase_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUseCase(mockRepo.Object, mockAuthService.Object);
            // Act
            var User = new UserBuilder()
                .WithLogin("testuser")
                .WithPasswordHash("ValidPasswordHash")
                .Build();
            var userLoginDto = new UserLoginDtoBuilder()
                .WithLogin("testuser")
                .WithPassword("InvalidPasswordHash")
                .Build();
            // Assert
            mockRepo.Setup(repo => repo.GetByLogin("testuser")).ReturnsAsync(User);
            mockAuthService.Setup(auth => auth.VerifyPassword(userLoginDto.password, User.PasswordHash)).Returns(false);
            var result = useCase.Execute(userLoginDto).Result;
            Assert.False(result.succes);
            Assert.Null(result.token);
        }
    }
}
