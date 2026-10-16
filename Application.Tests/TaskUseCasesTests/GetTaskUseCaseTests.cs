using Application.Interfaces;
using Application.Exceptions;
using Application.Tasks;
using Domain.Entities;
using Moq;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.TaskUseCasesTests
{
    public class GetTaskUseCaseTests
    {
        [Fact]
        public async Task GetTasksByRoomId_WhenRoomExists_ShouldReturnTasks()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var room = new RoomBuilder().Build();
            var tasks = new List<TaskItem?> { new TaskItemBuilder().WithRoomId(room.Id).Build() };

            mockRoomRepo.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);
            mockTaskRepo.Setup(repo => repo.GetTasksByRoomId(room.Id)).ReturnsAsync(tasks);

            var result = await useCase.GetTasksByRoomId(room.Id);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetTasksByRoomId_WhenRoomDoesNotExist_ShouldThrowRoomNotFoundException()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var roomId = Guid.NewGuid();

            mockRoomRepo.Setup(repo => repo.GetRoomById(roomId)).ReturnsAsync((Room?)null);

            await Assert.ThrowsAsync<RoomNotFoundException>(() => useCase.GetTasksByRoomId(roomId));

            mockTaskRepo.Verify(repo => repo.GetTasksByRoomId(It.IsAny<Guid>()), Times.Never);

        }

        [Fact]
        public async Task GetAllTasks_WhenRepositoryReturnsTasks_ShouldReturnTasks()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);
            var tasks = new List<TaskItem>
            {
                new TaskItemBuilder().Build(),
                new TaskItemBuilder().Build()
            };

            mockTaskRepo.Setup(repo => repo.GetAll()).ReturnsAsync(tasks);

            var result = await useCase.GetAllTasks();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task getTask_WhenTaskDoesNotExist_ShouldThrowTaskNotFoundException()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);
            var taskId = Guid.NewGuid();

            mockTaskRepo.Setup(repo => repo.GetTask(taskId)).ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<TaskNotFoundException>(() => useCase.getTask(taskId));

            mockTaskRepo.Verify(repo => repo.GetTask(It.IsAny<Guid>()), Times.Once);

        }

        [Fact]
        public async Task getTaskWithSubtasks_WhenTaskExists_ShouldReturnTask()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);
            var task = new TaskItemBuilder().Build();

            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);

            var result = await useCase.getTaskWithSubtasks(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
        }
    }
}
