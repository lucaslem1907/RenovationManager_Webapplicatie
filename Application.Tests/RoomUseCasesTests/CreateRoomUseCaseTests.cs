using Moq;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Shared.Builders.EntityBuilders;
using Shared.Builders.DtoBuilders;
using Application.Rooms;
using Application.Exceptions;

namespace Application.Tests.RoomUseCasesTests
{
    public class CreateRoomUseCaseTests
    {

        [Fact]
        public async Task Execute_WhenRoomIsCreated_ShouldReturnRoom()
        {
            // Arrange

            var mockRoomRepo = new Mock<IRoomRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();

            var useCase = new CreateRoomUseCase(mockRoomRepo.Object, mockProjectRepo.Object);

            var project = new ProjectBuilder()
                .WithName("Test Project")
                .Build();
            var roomDto = new RoomDtoBuilder()
                .WithName("Living Room")
                .WithStatus(RoomStatus.not_started)
                .Build();

            // Setup
            mockProjectRepo.Setup(r => r.GetById(project.Id)).ReturnsAsync(project);
            // Act
            var result = await useCase.Execute(project.Id, roomDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Living Room", result.Name);
            Assert.Equal(RoomStatus.not_started, result.Status);

            mockRoomRepo.Verify(r => r.Add(It.IsAny<Room>()), Times.Once);
            mockRoomRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenProjectIsNull_ShouldReturnException()
        {
            // Arrange
            var mockRoomRepo = new Mock<IRoomRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new CreateRoomUseCase(mockRoomRepo.Object, mockProjectRepo.Object);
            var projectId = Guid.NewGuid();
            var roomDto = new RoomDtoBuilder()
                .WithName("Living Room")
                .WithStatus(RoomStatus.not_started)
                .Build();

            // Setup
            mockProjectRepo.Setup(r => r.GetById(projectId)).ReturnsAsync((Project?)null);
            
            // Act
            await Assert.ThrowsAsync<ProjectNotFoundException>(() => useCase.Execute(projectId, roomDto));
            
            // Assert
            mockRoomRepo.Verify(r => r.Add(It.IsAny<Room>()), Times.Never);
            mockRoomRepo.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
