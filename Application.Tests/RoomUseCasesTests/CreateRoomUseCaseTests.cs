using Moq;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Shared.Builders.EntityBuilders;
using Shared.Builders.DtoBuilders;
using Application.Rooms;

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
            Assert.True(result.IsSuccess);
            Assert.Equal("Living Room", result.Value.Name);
            Assert.Equal(RoomStatus.not_started, result.Value.Status);

            mockRoomRepo.Verify(r => r.Add(It.IsAny<Room>()), Times.Once);
            mockRoomRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenProjectIsNull_ShouldReturnFailedResult()
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
            var result = await useCase.Execute(projectId, roomDto);
            Assert.True(result.IsFailed);
            
            // Assert
            mockRoomRepo.Verify(r => r.Add(It.IsAny<Room>()), Times.Never);
            mockRoomRepo.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
