using Application.Expenses;
using Application.Interfaces;
using Moq;
using Domain.Entities;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;
using Application.Exceptions;

namespace Application.Tests.ExpenseUseCasesTests
{
    public class DeleteExpenseUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenExpenseExist_ShouldDeleteExpense()
        {
            //Arrange
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var useCase = new DeleteExpenseUseCase(mockExpenseRepo.Object);
            var user = new UserBuilder().Build();
            var project = new ProjectBuilder().WithOwner(user).Build();
            var expense = new ExpenseBuilder().WithProjectId(project.Id).Build();
            //Setup
            mockExpenseRepo.Setup(repo => repo.GetExpenseById(expense.Id)).ReturnsAsync(expense);
            //Act
            await useCase.Execute(expense.Id);
            //Assert
            mockExpenseRepo.Verify(r => r.Delete(expense), Times.Once);
            mockExpenseRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenExpenseDoesNotExist_ShouldThrowExpenseNotFoundException()
        {
            //Arrange
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var useCase = new DeleteExpenseUseCase(mockExpenseRepo.Object);

            Guid expenseId = Guid.NewGuid();

            //Setup
            mockExpenseRepo.Setup(repo => repo.GetExpenseById(expenseId)).ReturnsAsync((Expense?)null);

            //Act & Assert
            await Assert.ThrowsAsync<ExpenseNotFoundException>(() => useCase.Execute(expenseId));
            mockExpenseRepo.Verify(r => r.Delete(It.IsAny<Expense>()), Times.Never);
            mockExpenseRepo.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
