using Application.Interfaces;
using Application.Tasks;
using Domain.Entities;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.SubtaskUseCasesTests
{
    public class CreateSubtaskUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenTaskExists_ShouldCreateSubtask()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new CreateSubTaskUseCase(mockSubtaskRepo.Object, mockTaskRepo.Object);

            var task = new TaskItemBuilder().Build();
            var dto = new SubTaskDtoBuilder().WithTitle("New subtask").WithIsCompleted(false).Build();

            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);

            var result = await useCase.Execute(task.Id, dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("New subtask", result.Value.Title);
            Assert.Equal(task.Id, result.Value.TaskItemId);
            mockSubtaskRepo.Verify(repo => repo.Add(result.Value), Times.Once);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenTaskDoesNotExist_ShouldReturnFailedResult()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new CreateSubTaskUseCase(mockSubtaskRepo.Object, mockTaskRepo.Object);
            var taskId = Guid.NewGuid();
            var dto = new SubTaskDtoBuilder().Build();

            mockTaskRepo.Setup(repo => repo.GetTask(taskId)).ReturnsAsync((TaskItem?)null);

            var result = await useCase.Execute(taskId, dto);
            Assert.True(result.IsFailed);

            mockSubtaskRepo.Verify(repo => repo.Add(It.IsAny<Subtask>()), Times.Never);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Never);
        }
    }
}
