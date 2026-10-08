using Application.Exceptions;
using Application.Interfaces;
using Application.Projects;
using Domain.Entities;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.ProjectUseCasesTests;

public class CreateProjectUseCaseTests
{
    [Fact]
    public async Task Execute_whenUserExists_ShouldCreateProject()
    {
        var mockProjectRepo = new Mock<IProjectRepository>();
        var mockUserRepo = new Mock<IUserRepository>();

        var useCase = new CreateProjectUseCase(mockProjectRepo.Object, mockUserRepo.Object);

        var testUser = new UserBuilder().Build();
        var projectDto = new ProjectDtoBuilder()
            .WithBudget(1000)
            .WithOwnerId(testUser.Id)
            .Build();

        mockUserRepo.Setup(mockUserRepo => mockUserRepo.GetById(testUser.Id)).ReturnsAsync(testUser);

        var result = await useCase.Execute(projectDto);

        Assert.NotNull(result);
        Assert.Equal(projectDto.Name, result.Name);
        Assert.Equal(projectDto.Budget, result.Budget);

        mockProjectRepo.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);
        mockProjectRepo.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Execute_whenUserNotExists_ShouldThrowOwnerNotFoundException()
    {
        var mockProjectRepo = new Mock<IProjectRepository>();
        var mockUserRepo = new Mock<IUserRepository>();

        var useCase = new CreateProjectUseCase(mockProjectRepo.Object, mockUserRepo.Object);

        var ownerId = Guid.NewGuid();
        var projectDto = new ProjectDtoBuilder()
            .WithOwnerId(ownerId)
            .Build();

        mockUserRepo.Setup(repo => repo.GetById(ownerId)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<OwnerNotFoundException>(() => useCase.Execute(projectDto));

        mockProjectRepo.Verify(r => r.Add(It.IsAny<Project>()), Times.Never);
        mockProjectRepo.Verify(r => r.SaveChanges(), Times.Never);
    }

}
