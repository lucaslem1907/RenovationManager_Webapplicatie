using Domain.Entities;
using Shared.DTO;
using Application.Interfaces;
using Application.Projects;
using Moq;

namespace Application.Tests.ProjectTestUseCases;

public class CreateProjectUseCaseTests
{
    [Fact]
    public async Task Execute_whenUserExists_ShouldCreateProject()
    {   //Arrange:
        //dependencies en inputs
        var mockProjectRepo = new Mock<IProjectRepository>();
        var mockUserRepo = new Mock<IUserRepository>();

        var UseCase = new CreateProjectUseCase(mockProjectRepo.Object, mockUserRepo.Object);

        var testUser = new User("Lucas", "test", "lucas.test@test.com", "hash");
        var ProjectDto = new ProjectDto
        {
            Name = "New Kitchen",
            OwnerId = testUser.Id,
            Address = "Street 1",
            Description = "Fixing the kitchen"
        };

        //ophalen testuser 
        mockUserRepo.Setup(mockUserRepo => mockUserRepo.GetById(testUser.Id)).ReturnsAsync(testUser);

        //Act: uit testen code en zie of hij fout teruggeeft
        var result = await UseCase.Execute(ProjectDto);

        //Assert: verifiëren van resultaten
        Assert.NotNull(result);
        Assert.Equal("New Kitchen", result.Name);

        //verifiëren of repo's add en savechanges worden gebruikt
        mockProjectRepo.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);    
        mockProjectRepo.Verify(r => r.SaveChanges(), Times.Once);



    }

}
