using Application.Exceptions;
using Application.Interfaces;
using Application.Tasks;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.TaskUseCasesTests
{
    public class CreateTaskUseCasesTests
    {

        [Fact]
        public async Task Execute_WhenTaskIsCreated_ShouldReturnTask()
        {
            //Arrange
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new CreateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var room = new RoomBuilder()
                .WithName("Test Room")
                .Build();
            var taskDto = new TaskDtoBuilder()
                .WithTitle("New Task")
                .Build();

            //Setup
            mockRoomRepo.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);

            //Act
            var result = await useCase.Execute(room.Id, taskDto);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("New Task", result.Title);
            Assert.Equal(room.Id, result.RoomId);

            mockTaskRepo.Verify(r => r.Add(result), Times.Once);
            mockTaskRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenRoomNotFound_ShouldThrowRoomNotFoundException()
        {
            //Arrange
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new CreateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var roomId = Guid.NewGuid();
            var taskDto = new TaskDtoBuilder()
                .WithTitle("New Task")
                .Build();

            //Setup
            mockRoomRepo.Setup(repo => repo.GetRoomById(roomId)).ReturnsAsync((Room)null);

            //Act & Assert
            await Assert.ThrowsAsync<RoomNotFoundException>(() => useCase.Execute(roomId, taskDto));
        }

        [Fact]
        public async Task Execute_WhenRoomIsDone_ShouldMarkRoomInProgress()
        {
            //Arrange
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new CreateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var room = new RoomBuilder()
                .WithName("Test Room")
                .WithStatus(RoomStatus.done)
                .Build();

            var taskDto = new TaskDtoBuilder()
                .WithTitle("New Task")
                .Build();
            //Setup
            mockRoomRepo.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);

            //Act
            var result = await useCase.Execute(room.Id, taskDto);

            //Assert
            Assert.Equal(RoomStatus.in_progress, room.Status);
        }
    }
}
