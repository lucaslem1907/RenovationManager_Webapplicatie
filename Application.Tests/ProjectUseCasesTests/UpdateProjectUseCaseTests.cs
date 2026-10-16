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
            Assert.True(result.IsSuccess);
            Assert.Equal("Aangepaste Naam", result.Value.Name);
            Assert.Equal("Aangepast Adres", result.Value.Address);
            Assert.Equal("toevoegen beschrijving", result.Value.Description);
            Assert.Equal(10000, result.Value.Budget);
            Assert.Equal(DateTime.Now.Date, result.Value.StartDate.Date);

            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Once);


        }

        [Fact]
        public async Task Execute_WhenProjectDoesNotExist_ShouldReturnFailedResult()
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
            mockProjectRepo.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Project?)null);

            //Act & Assert
            var result = await useCase.Execute(Guid.NewGuid(), ProjectDto);
            Assert.True(result.IsFailed);
            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Never);

        }
    }
}
