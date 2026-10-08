using Application.Exceptions;
using Application.Interfaces;
using Application.Projects;
using Domain.Entities;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.ProjectTestUseCases
{
    public class UpdateProjectUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenProjectExist_ShouldUpdateProject()
        {
            //Arrange
            var mockProjectRepo = new Mock<IProjectRepository>();

            var useCase = new UpdateProjectUseCase(mockProjectRepo.Object);

            var user = new UserBuilder().Build();
            var project = new ProjectBuilder().WithOwner(user).Build();

            var ProjectDto = new ProjectDtoBuilder()
                .WithOwnerId(user.Id)
                .WithName("Aangepaste Naam")
                .WithAddress("Aangepast Adres")
                .WithDescription("toevoegen beschrijving")
                .WithBudget(10000)
                .WithStartDate(DateTime.Now)
                .Build();

            //Setup
            mockProjectRepo.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);

            //Act
            var result = await useCase.Execute(project.Id, ProjectDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Aangepaste Naam", result.Name);
            Assert.Equal("Aangepast Adres", result.Address);
            Assert.Equal("toevoegen beschrijving", result.Description);
            Assert.Equal(10000, result.Budget);
            Assert.Equal(DateTime.Now.Date, result.StartDate.Date);

            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Once);


        }

        [Fact]
        public async Task Execute_WhenProjectDoesNotExist_ShouldThrowProjectNotFoundException()
        {
            //Arrange 
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new UpdateProjectUseCase(mockProjectRepo.Object);

            var ProjectDto = new ProjectDtoBuilder()
                .WithName("Aangepaste Naam")
                .WithAddress("Aangepast Adres")
                .WithDescription("toevoegen beschrijving")
                .WithBudget(10000)
                .WithStartDate(DateTime.Now)
                .Build();
            //setup
            mockProjectRepo.Setup(repo => repo.GetByIdWithDetails(It.IsAny<Guid>())).ReturnsAsync((Project?)null);

            //Act & Assert
            await Assert.ThrowsAsync<ProjectNotFoundException>(() => useCase.Execute(Guid.NewGuid(), ProjectDto));
            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Never);

        }
    }
}
