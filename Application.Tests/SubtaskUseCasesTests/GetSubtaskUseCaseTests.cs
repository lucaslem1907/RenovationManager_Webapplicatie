using Application.Interfaces;
using Application.Subtaks;
using Application.Exceptions;
using Domain.Entities;
using Moq;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.SubtaskUseCasesTests
{
    public class GetSubtaskUseCaseTests
    {
        [Fact]
        public async Task GetSubTask_WhenExists_ShouldReturnSubtask()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var useCase = new GetSubtaskUseCase(mockSubtaskRepo.Object);
            var subtask = new SubtaskBuilder().Build();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtask.Id)).ReturnsAsync(subtask);

            var result = await useCase.GetSubTask(subtask.Id);

            Assert.NotNull(result);
            Assert.Equal(subtask.Id, result.Id);
        }

        [Fact]
        public async Task GetAllTasks_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var useCase = new GetSubtaskUseCase(mockSubtaskRepo.Object);

            mockSubtaskRepo.Setup(repo => repo.GetAll()).ReturnsAsync((IEnumerable<Subtask>)null);

            var result = await useCase.GetAllTasks();

            Assert.Null(result);
        }

        [Fact]
        public async Task getSubTask_WhenMissing_ShouldReturnNull()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var useCase = new GetSubtaskUseCase(mockSubtaskRepo.Object);
            var subtaskId = Guid.NewGuid();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtaskId)).ReturnsAsync((Subtask?)null);

            await Assert.ThrowsAsync<SubTaskNotFoundException>(() => useCase.GetSubTask(subtaskId));
            mockSubtaskRepo.Verify(repo => repo.GetSubTask(subtaskId), Times.Once);
        }
    }
}
