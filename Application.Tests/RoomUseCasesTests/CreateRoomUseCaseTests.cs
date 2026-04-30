using Moq;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Shared.DTO;
using Application.Rooms;
using AutoFixture;

namespace Application.Tests.RoomUseCasesTests
{
    public class CreateRoomUseCaseTests
    {
        Fixture fixture = new Fixture();

        [Fact]
        public async Task Execute_WhenRoomIsCreated_ShouldReturnRoom()
        {
            // Arrange

            var mockRoomRepo = new Mock<IRoomRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();

            var useCase = new CreateRoomUseCase(mockRoomRepo.Object, mockProjectRepo.Object);
            var roomDto = new RoomDto
            {
                Name = "Living Room",
                Status = RoomStatus.not_started,
            };
            var project = fixture.Create<Project>();
            // Setup: Simuleer dat het project bestaat
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
        public async Task Execute_WhenProjectIsNull_ShouldReturnNull()
        {
            // Arrange
            var mockRoomRepo = new Mock<IRoomRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new CreateRoomUseCase(mockRoomRepo.Object, mockProjectRepo.Object);
            var roomDto = fixture.Create<RoomDto>();
            var projectId = Guid.NewGuid();

            // Setup: Simuleer dat het project niet bestaat
            mockProjectRepo.Setup(r => r.GetById(projectId)).ReturnsAsync((Project?)null);
            
            // Act
            var result = await useCase.Execute(projectId, roomDto);
            
            // Assert
            Assert.Null(result);
            mockRoomRepo.Verify(r => r.Add(It.IsAny<Room>()), Times.Never);
            mockRoomRepo.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
