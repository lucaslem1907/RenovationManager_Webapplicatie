using Moq;
using Xunit;
using Application.Interfaces;
using Application.Projects;
using Domain.Entities;

namespace Application.Tests.ProjectTestUseCases
{
    public class DeleteProjectUseCaseTest
    {
        [Fact]
        public async Task Execute_whenProjectExists_ShouldDeleteProject()
        {
            
            // dependencies en inputs
            var projectRepoMock = new Mock<IProjectRepository>();
            var useCase = new DeleteProjectUseCase(projectRepoMock.Object);
            var user = new User(
                "Test User",
                "Achternaam",
                "Email@email.com",
                "Adress"
                );
            var project = new Project(
                "Test Project",
                user, 
                "adress"
                );

            projectRepoMock.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);

            //act 
            var result = await useCase.Execute(project.Id);

            //assert
            Assert.True( result );

            projectRepoMock.Verify(r => r.Delete(project), Times.Once);
            projectRepoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_whenProjectNotExists_ShouldNotDeleteProject()
        {
            //Arrange:
            // dependencies en inputs
            var projectRepoMock = new Mock<IProjectRepository>();
            var useCase = new DeleteProjectUseCase(projectRepoMock.Object);
            var nonExistingProjectId = Guid.NewGuid();

            //setup: geef een null terug om aan te geven dat het project niet bestaat
            projectRepoMock.Setup(repo => repo.GetById(nonExistingProjectId)).ReturnsAsync((Project)null!);

            //act 
            var result = await useCase.Execute(nonExistingProjectId);

            //assert 
            // dit moet false zijn omdat er geen project is om te verwijderen
            Assert.False(result);

            projectRepoMock.Verify(r => r.Delete(It.IsAny<Project>()), Times.Never);
            projectRepoMock.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
