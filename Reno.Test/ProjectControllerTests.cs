using Application.Interfaces;
using Application.Projects;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Reno.Controllers;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;
using Xunit;

namespace Reno.Test.ProjectControllerTests;

public class ProjectControllerTests
{
    [Fact]
    public async Task CreateProject_WhenOwnerExists_ShouldReturnOk()
    {
        var projectRepoMock = new Mock<IProjectRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var exportMock = new Mock<IExportExcelService>();

        var controller = CreateController(projectRepoMock, userRepoMock, exportMock);

        var user = new UserBuilder().Build();
        var dto = new ProjectDtoBuilder().WithOwnerId(user.Id).Build();

        userRepoMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);

        var result = await controller.CreateProject(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<Project>(okResult.Value);
        Assert.Equal(dto.Name, value.Name);

        projectRepoMock.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);
        projectRepoMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task CreateProject_WhenOwnerMissing_ShouldReturnNotFound()
    {
        var projectRepoMock = new Mock<IProjectRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var exportMock = new Mock<IExportExcelService>();

        var controller = CreateController(projectRepoMock, userRepoMock, exportMock);

        var missingOwnerId = Guid.NewGuid();
        var dto = new ProjectDtoBuilder().WithOwnerId(missingOwnerId).Build();

        userRepoMock.Setup(x => x.GetById(missingOwnerId)).ReturnsAsync((User?)null);

        var result = await controller.CreateProject(dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Owner not found for id {missingOwnerId}", notFound.Value);

        projectRepoMock.Verify(r => r.Add(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public async Task GetProject_WhenProjectMissing_ShouldReturnNotFound()
    {
        var projectRepoMock = new Mock<IProjectRepository>();
        var userRepoMock = new Mock<IUserRepository>();
        var exportMock = new Mock<IExportExcelService>();

        var controller = CreateController(projectRepoMock, userRepoMock, exportMock);

        var projectId = Guid.NewGuid();
        projectRepoMock.Setup(x => x.GetById(projectId)).ReturnsAsync((Project?)null);

        var result = await controller.GetProject(projectId);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"Project not found for id {projectId}", notFound.Value);
    }

    private static ProjectController CreateController(
        Mock<IProjectRepository> projectRepoMock,
        Mock<IUserRepository> userRepoMock,
        Mock<IExportExcelService> exportMock)
    {
        var createUseCase = new CreateProjectUseCase(projectRepoMock.Object, userRepoMock.Object);
        var getUseCase = new GetProjectUseCase(projectRepoMock.Object);
        var updateUseCase = new UpdateProjectUseCase(projectRepoMock.Object);
        var deleteUseCase = new DeleteProjectUseCase(projectRepoMock.Object);
        var generateExcelUseCase = new GenerateProjectExcel(exportMock.Object, projectRepoMock.Object);

        return new ProjectController(createUseCase, getUseCase, updateUseCase, deleteUseCase, generateExcelUseCase);
    }
}
