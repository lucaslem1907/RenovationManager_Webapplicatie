using Application.Interfaces;
using Application.Exceptions;
using Application.Subtaks;
using Domain.Entities;
using Moq;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.SubtaskUseCasesTests
{
    public class DeleteSubtaskUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenSubtaskExists_ShouldDeleteSubtask()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var useCase = new DeleteSubtaskUseCase(mockSubtaskRepo.Object);
            var subtask = new SubtaskBuilder().Build();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtask.Id)).ReturnsAsync(subtask);

            var result = await useCase.Execute(subtask.Id);

            Assert.True(result);
            mockSubtaskRepo.Verify(repo => repo.Delete(subtask), Times.Once);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenSubtaskMissing_ShouldThrowSubTaskNotFoundException()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var useCase = new DeleteSubtaskUseCase(mockSubtaskRepo.Object);
            var subtaskId = Guid.NewGuid();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtaskId)).ReturnsAsync((Subtask?)null);

            await Assert.ThrowsAsync<SubTaskNotFoundException>(() => useCase.Execute(subtaskId));

            mockSubtaskRepo.Verify(repo => repo.Delete(It.IsAny<Subtask>()), Times.Never);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Never);
        }
    }
}
