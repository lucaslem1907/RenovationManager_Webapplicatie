using Domain.Entities;
using Domain.Enums;
using Shared.DTO;
using Application.Interfaces;
using Application.Rooms;
using Moq;
using Shouldly;
using AutoFixture;



namespace Application.Tests.RoomUseCasesTests
{
    
    public class UpdateRoomUseCaseTest
    {
        Fixture fixture = new Fixture();
        [Fact]
        public async Task Execute_WhenRoomExists_ShouldUpdateRoom()
        {
            // Arrange
            var mockRepo = new Mock<IRoomRepository>();
            var useCase = new UpdateRoomUseCase(mockRepo.Object);
            var roomId = Guid.NewGuid();
            var existingRoom = fixture.Create<Room>();
            var roomDto = new RoomDto { Name = "New Name", Status = RoomStatus.in_progress };
            // Setup: Simuleer dat de kamer bestaat
            mockRepo.Setup(r => r.GetRoomById(roomId)).ReturnsAsync(existingRoom);
            // Act
            var result = await useCase.Execute(roomId, roomDto);
            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("New Name");
            result.Status.ShouldBe(RoomStatus.in_progress);

            mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
