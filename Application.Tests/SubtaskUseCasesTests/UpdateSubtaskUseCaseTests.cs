using Application.Interfaces;
using Application.Subtaks;
using Application.Exceptions;
using Domain.Entities;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;

namespace Application.Tests.SubtaskUseCasesTests
{
    public class UpdateSubtaskUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenSubtaskDoesNotExist_ShouldThrowSubTaskNotFoundException()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new UpdateSubtaskUseCase(mockSubtaskRepo.Object, mockTaskRepo.Object);
            var subtaskId = Guid.NewGuid();
            var dto = new SubTaskDtoBuilder().Build();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtaskId)).ReturnsAsync((Subtask?)null);
            
            await Assert.ThrowsAsync<SubTaskNotFoundException>(() => useCase.Execute(subtaskId, dto));

            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenAllSubtasksCompleted_ShouldMarkParentTaskCompleted()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new UpdateSubtaskUseCase(mockSubtaskRepo.Object, mockTaskRepo.Object);

            var task = new TaskItemBuilder().WithIsCompleted(false).Build();
            var subtaskToUpdate = new SubtaskBuilder().WithTaskItemId(task.Id).WithIsCompleted(false).Build();
            task.Subtasks.Add(subtaskToUpdate);

            var dto = new SubTaskDtoBuilder().WithTitle("Done subtask").WithIsCompleted(true).Build();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtaskToUpdate.Id)).ReturnsAsync(subtaskToUpdate);
            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);

            var result = await useCase.Execute(subtaskToUpdate.Id, dto);

            Assert.NotNull(result);
            Assert.True(task.IsCompleted);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenIncompleteSubtasksRemain_ShouldKeepParentTaskIncomplete()
        {
            var mockSubtaskRepo = new Mock<ISubtaskRepository>();
            var mockTaskRepo = new Mock<ITaskRepository>();
            var useCase = new UpdateSubtaskUseCase(mockSubtaskRepo.Object, mockTaskRepo.Object);

            var task = new TaskItemBuilder().WithIsCompleted(false).Build();
            var subtaskToUpdate = new SubtaskBuilder().WithTaskItemId(task.Id).WithIsCompleted(true).Build();
            var remainingIncomplete = new SubtaskBuilder().WithTaskItemId(task.Id).WithIsCompleted(false).Build();
            task.Subtasks.Add(subtaskToUpdate);
            task.Subtasks.Add(remainingIncomplete);

            var dto = new SubTaskDtoBuilder().WithTitle("Still done").WithIsCompleted(true).Build();

            mockSubtaskRepo.Setup(repo => repo.GetSubTask(subtaskToUpdate.Id)).ReturnsAsync(subtaskToUpdate);
            mockTaskRepo.Setup(repo => repo.GetTask(task.Id)).ReturnsAsync(task);

            var result = await useCase.Execute(subtaskToUpdate.Id, dto);

            Assert.NotNull(result);
            Assert.False(task.IsCompleted);
            mockSubtaskRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}
