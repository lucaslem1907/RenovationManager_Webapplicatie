using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.Exceptions;
using Shared.Builders.EntityBuilders;
using Shared.Builders.DtoBuilders;
using Application.Rooms;
using Moq;
using Shouldly;




namespace Application.Tests.RoomUseCasesTests
{
    
    public class UpdateRoomUseCaseTest
    {
        [Fact]
        public async Task Execute_WhenRoomExists_ShouldUpdateRoom()
        {
            // Arrange
            var mockRepo = new Mock<IRoomRepository>();
            var useCase = new UpdateRoomUseCase(mockRepo.Object);

            var existingRoom = new RoomBuilder()
                .WithName("Old Name")
                .WithStatus(RoomStatus.not_started)
                .Build();
            var roomDto = new RoomDtoBuilder()
                .WithName("New Name")
                .WithStatus(RoomStatus.in_progress)
                .Build();

            // Setup: Simuleer dat de kamer bestaat
            mockRepo.Setup(r => r.GetRoomById(existingRoom.Id)).ReturnsAsync(existingRoom);
            // Act
            var result = await useCase.Execute(existingRoom.Id, roomDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", existingRoom.Name);
            Assert.Equal(RoomStatus.in_progress, existingRoom.Status);

            mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
