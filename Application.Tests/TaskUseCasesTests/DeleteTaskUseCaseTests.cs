using Application.Interfaces;
using Application.Tasks;
using Domain.Entities;
using Moq;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.TaskUseCasesTests
{
    public class DeleteTaskUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenTaskExists_ShouldDeleteTask()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new DeleteTaskUseCase(mockTaskRepo.Object);
            var task = new TaskItemBuilder().Build();

            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);

            var result = await useCase.Execute(task.Id);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            mockTaskRepo.Verify(repo => repo.Delete(task), Times.Once);
            mockTaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenTaskDoesNotExist_ShouldReturnFailedResult()
        {
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new DeleteTaskUseCase(mockTaskRepo.Object);
            var taskId = Guid.NewGuid();

            mockTaskRepo.Setup(repo => repo.GetTask(taskId)).ReturnsAsync((TaskItem?)null);
            
            var result = await useCase.Execute(taskId);
            Assert.True(result.IsFailed);

            mockTaskRepo.Verify(repo => repo.Delete(It.IsAny<TaskItem>()), Times.Never);
            mockTaskRepo.Verify(repo => repo.SaveChanges(), Times.Never);
        }
    }
}
