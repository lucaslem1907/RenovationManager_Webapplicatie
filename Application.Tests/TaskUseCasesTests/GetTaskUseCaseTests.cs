using Application.Interfaces;
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

            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task GetTasksByRoomId_WhenRoomDoesNotExist_ShouldReturnFailedResult()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);

            var roomId = Guid.NewGuid();

            mockRoomRepo.Setup(repo => repo.GetRoomById(roomId)).ReturnsAsync((Room?)null);

            var result = await useCase.GetTasksByRoomId(roomId);
            Assert.True(result.IsFailed);

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

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task getTask_WhenTaskDoesNotExist_ShouldReturnFailedResult()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var mockRoomRepo = new Mock<IRoomRepository>();
            var useCase = new GetTaskUseCase(mockTaskRepo.Object, mockRoomRepo.Object);
            var taskId = Guid.NewGuid();

            mockTaskRepo.Setup(repo => repo.GetTask(taskId)).ReturnsAsync((TaskItem?)null);

            var result = await useCase.getTask(taskId);
            Assert.True(result.IsFailed);

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

            Assert.True(result.IsSuccess);
            Assert.Equal(task.Id, result.Value.Id);
        }
    }
}
