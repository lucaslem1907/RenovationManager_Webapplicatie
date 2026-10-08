using Application.Interfaces;
using Application.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.TaskUseCasesTests
{
    public class UpdateTaskUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenTaskDoesNotExist_ShouldThrowTaskNotFoundException()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);
            var taskId = Guid.NewGuid();
            var dto = new TaskDtoBuilder().WithTitle("Updated").Build();

            mockTaskRepo.Setup(repo => repo.GetTask(taskId)).ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<TaskNotFoundException>(() => useCase.Execute(taskId, dto));

            mockTaskRepo.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenAllRoomTasksCompleted_ShouldMarkRoomDone()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var room = new RoomBuilder().WithStatus(RoomStatus.in_progress).Build();
            var task = new TaskItemBuilder().WithRoomId(room.Id).WithIsCompleted(false).Build();
            room.Tasks.Add(task);

            var dto = new TaskDtoBuilder()
                .WithTitle("Updated task")
                .WithDescription("Updated description")
                .WithIsCompleted(true)
                .Build();

            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);
            mockRoomRepo.Setup(repo => repo.GetRoomWithTaskAndSubTasks(room.Id)).ReturnsAsync(room);

            var result = await useCase.Execute(task.Id, dto);

            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.Equal(RoomStatus.done, room.Status);
            mockTaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenRoomHasIncompleteTasksAndRoomWasDone_ShouldMarkRoomInProgress()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new UpdateTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var room = new RoomBuilder().WithStatus(RoomStatus.done).Build();
            var taskToUpdate = new TaskItemBuilder().WithRoomId(room.Id).WithIsCompleted(false).Build();
            var stillIncompleteTask = new TaskItemBuilder().WithRoomId(room.Id).WithIsCompleted(false).Build();
            room.Tasks.Add(taskToUpdate);
            room.Tasks.Add(stillIncompleteTask);

            var dto = new TaskDtoBuilder()
                .WithTitle("Task updated")
                .WithDescription("Task description updated")
                .WithIsCompleted(false)
                .Build();

            mockTaskRepo.Setup(repo => repo.GetTask(taskToUpdate.Id)).ReturnsAsync(taskToUpdate);
            mockRoomRepo.Setup(repo => repo.GetRoomWithTaskAndSubTasks(room.Id)).ReturnsAsync(room);

            var result = await useCase.Execute(taskToUpdate.Id, dto);

            Assert.NotNull(result);
            Assert.Equal(RoomStatus.in_progress, room.Status);
            mockTaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}
