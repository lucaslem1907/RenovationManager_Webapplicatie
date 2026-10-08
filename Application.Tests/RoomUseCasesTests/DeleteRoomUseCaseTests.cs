using Application.Exceptions;
using Application.Interfaces;
using Shared.Builders.EntityBuilders;
using Shared.Builders.DtoBuilders;
using Application.Rooms;
using Domain.Entities;
using Moq;
using Domain.Enums;
using Xunit;
namespace Application.Tests.RoomUseCasesTests
{
    public class DeleteRoomUseCaseTests
    {

        [Fact]
        public async Task Execute_WhenRoomExists_ShouldDeleteRoom_ShouldDeleteAssociatedExpenses()
        {
            // Arrange
            var mockRepo = new Mock<IRoomRepository>();
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var useCase = new DeleteRoomUseCase(mockRepo.Object, mockExpenseRepo.Object);

            var existingRoom = new RoomBuilder()
                .WithName("Room to Delete")
                .WithStatus(RoomStatus.not_started)
                .Build();

            var expense = new ExpenseBuilder()
                .WithRoomId(existingRoom.Id)
                .Build();

            // Setup: Simuleer dat de kamer bestaat
            mockRepo.Setup(r => r.GetRoomById(existingRoom.Id)).ReturnsAsync(existingRoom);
            mockExpenseRepo.Setup(r => r.GetExpensesByRoomId(existingRoom.Id)).ReturnsAsync(new List<Expense> { expense });

            // Act
            await useCase.Execute(existingRoom.Id, true);
            // Assert
            mockRepo.Verify(r => r.Delete(existingRoom), Times.Once);
            mockExpenseRepo.Verify(r => r.DeleteRange(It.Is<IEnumerable<Expense>>(expenses => expenses.Contains(expense))), Times.Once);
            mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenRoomExists_ShouldDeleteRoom_ShouldSetAssociatedExpesesNull()
        {            
            // Arrange
            var mockRepo = new Mock<IRoomRepository>();
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var useCase = new DeleteRoomUseCase(mockRepo.Object, mockExpenseRepo.Object);

            var existingRoom = new RoomBuilder()
                .WithName("Room to Delete")
                .WithStatus(RoomStatus.not_started)
                .Build();

            var expense = new ExpenseBuilder()
                .WithRoomId(existingRoom.Id)
                .Build();

            // Setup
            mockRepo.Setup(r => r.GetRoomById(existingRoom.Id)).ReturnsAsync(existingRoom);
            mockExpenseRepo.Setup(r => r.GetExpensesByRoomId(existingRoom.Id)).ReturnsAsync(new List<Expense> { expense });

            // Act
            await useCase.Execute(existingRoom.Id, false);
            // Assert
            mockRepo.Verify(r => r.Delete(existingRoom), Times.Once);
            mockExpenseRepo.Verify(r => r.DeleteRange(It.IsAny<IEnumerable<Expense>>()), Times.Never);
            mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task DoNotExecute_WhenRoomDoesNotExist_ShouldThrowRoomNotFoundException()
        {
            // Arrange
            var mockRepo = new Mock<IRoomRepository>();
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var useCase = new DeleteRoomUseCase(mockRepo.Object, mockExpenseRepo.Object);

            var nonExistentRoomId = Guid.NewGuid();

            // Setup
            mockRepo.Setup(r => r.GetRoomById(nonExistentRoomId)).ReturnsAsync((Room?)null);

            // Act & Assert
            await Assert.ThrowsAsync<RoomNotFoundException>(() => useCase.Execute(nonExistentRoomId, true));

            mockRepo.Verify(r => r.Delete(It.IsAny<Room>()), Times.Never);
            mockExpenseRepo.Verify(r => r.DeleteRange(It.IsAny<IEnumerable<Expense>>()), Times.Never);
            mockRepo.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
