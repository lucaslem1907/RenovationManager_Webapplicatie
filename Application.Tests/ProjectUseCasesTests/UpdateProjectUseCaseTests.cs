using Application.Interfaces;
using Application.Projects;
using Shared.DTO;
using Domain.Entities;
using Moq;

namespace Application.Tests.ProjectTestUseCases
{
    public class UpdateProjectUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenProjectExist_ShouldUpdateProject()
        {
            //definiëren van dependencies en input 
            var mockProjectRepo = new Mock<IProjectRepository>();

            var useCase = new UpdateProjectUseCase(mockProjectRepo.Object);

            var user = new User("Voornaam", "Achternaam", "voornaam.achternaam@email.com", "passwordHash");
            var project = new Project("oude naam", user, "oud adress");

            var ProjectDto = new ProjectDto
            {
                Name = "Aangepaste Naam",
                OwnerId = user.Id,
                Address = "Aangepast Adres",
                Description = "toevoegen beschrijving",
                Budget = 10000,
                StartDate = DateTime.Now

            };

            //ophalen van project
            mockProjectRepo.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);

            //Act
            var result = await useCase.Execute(project.Id, ProjectDto);

            //Assert: verifiëren van resultaten
            Assert.NotNull(result);
            Assert.Equal("Aangepaste Naam", result.Name);
            Assert.Equal("Aangepast Adres", result.Address);
            Assert.Equal("toevoegen beschrijving", result.Description);
            Assert.Equal(10000, result.Budget);
            Assert.Equal(DateTime.Now.Date, result.StartDate.Date);
            //is alles toegevoegd op het object?
            Assert.Equal("Aangepaste Naam", project.Name);

            //verifiëren savechanges worden gebruikt
            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Once);


        }

        [Fact]
        public async Task Execute_WhenProjectDoesNotExist_ShouldReturnNull()
        {
            //definiëren van dependencies en input 
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new UpdateProjectUseCase(mockProjectRepo.Object);
            var ProjectDto = new ProjectDto
            {
                Name = "Aangepaste Naam",
                OwnerId = Guid.NewGuid(),
                Address = "Aangepast Adres",
                Description = "toevoegen beschrijving",
                Budget = 10000,
                StartDate = DateTime.Now
            };
            //ophalen van project
            mockProjectRepo.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
            //Act
            var result = await useCase.Execute(Guid.NewGuid(), ProjectDto);
            //Assert: verifiëren van resultaten
            Assert.Null(result);
            //verifiëren savechanges worden niet gebruikt
            mockProjectRepo.Verify(r => r.SaveChanges(), Times.Never);

        }
    }
}
