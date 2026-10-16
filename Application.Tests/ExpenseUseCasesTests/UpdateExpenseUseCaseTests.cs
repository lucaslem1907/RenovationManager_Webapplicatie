using Application.Expenses;
using Application.Interfaces;
using Moq;
using Domain.Entities;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;
using Application.Exceptions;

namespace Application.Tests.ExpenseUseCasesTests
{
    public class UpdateExpenseUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenExpenseExist_ShouldUpdateExpense()
        {
            //Arrange
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new UpdateExpenseUseCase(mockExpenseRepo.Object, mockProjectRepo.Object);


            var project = new ProjectBuilder().WithBudget(100).Build();
            var expense = new ExpenseBuilder().WithProjectId(project.Id).WithAmount(30).Build();

            var expenseDto = new ExpenseDtoBuilder()
                .WithName("Updated Expense")
                .WithAmount(50)
                .WithCreatedDate(DateTime.Now.Date)
                .Build();

            //Setup
            mockExpenseRepo.Setup(repo => repo.GetExpenseById(expense.Id)).ReturnsAsync(expense);
            mockProjectRepo.Setup(repo => repo.GetById(expense.ProjectId)).ReturnsAsync(project);
            mockExpenseRepo.Setup(repo => repo.GetExpensesByProjectId(project.Id)).ReturnsAsync(new List<Expense?> { expense });
            
            //Act
            var result = await useCase.Execute(expense.Id, expenseDto);
            //Assert
            Assert.True(result.IsSuccess);
            var UpdatedExpense = result.Value;

            Assert.NotNull(UpdatedExpense);
            Assert.Equal("Updated Expense", UpdatedExpense.Name);
            Assert.Equal(50, UpdatedExpense.Amount);
            Assert.Equal(DateTime.Now.Date, UpdatedExpense.CreatedDate);

            mockExpenseRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenExpenseDoesNotExist_ShouldThrowExpenseNotFoundException()
        {
            //Arrange
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new UpdateExpenseUseCase(mockExpenseRepo.Object, mockProjectRepo.Object);

            Guid expense = Guid.NewGuid();
            var expenseDto = new ExpenseDtoBuilder()
                .WithName("Updated Expense")
                .WithAmount(200)
                .WithCreatedDate(DateTime.Now)
                .Build();

            //Setup
            mockExpenseRepo.Setup(repo => repo.GetExpenseById(expense)).ReturnsAsync((Expense?)null);

            //Act & Assert
            var result = await useCase.Execute(expense, expenseDto);

            Assert.False(result.IsSuccess);
            Assert.True(result.Errors.First() is ExpenseNotFoundError);
            mockExpenseRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task NotExecute_WhenBudgetIsExceeded_ShouldThrowBudgetExceededException()
        {
            //Arrange
            var mockExpenseRepo = new Mock<IExpenseRepository>();
            var mockProjectRepo = new Mock<IProjectRepository>();
            var useCase = new UpdateExpenseUseCase(mockExpenseRepo.Object, mockProjectRepo.Object);

            var project = new ProjectBuilder().WithBudget(100).Build();
            var expense = new ExpenseBuilder()
                .WithProjectId(project.Id)
                .WithAmount(50)
                .Build();

            var expenseDto = new ExpenseDtoBuilder()
                .WithName("Updated Expense")
                .WithAmount(200)
                .WithCreatedDate(DateTime.Now)
                .Build();

            //Setup
            mockExpenseRepo.Setup(repo => repo.GetExpenseById(expense.Id)).ReturnsAsync(expense);
            mockProjectRepo.Setup(repo => repo.GetById(expense.ProjectId)).ReturnsAsync(project);
            mockExpenseRepo.Setup(repo => repo.GetExpensesByProjectId(project.Id)).ReturnsAsync(new List<Expense> { expense });


            //Act & Assert
            var result = await useCase.Execute(expense.Id, expenseDto);

            Assert.False(result.IsSuccess);
            Assert.True(result.Errors.First() is BudgetExceededError);

            mockExpenseRepo.Verify(r => r.SaveChanges(), Times.Never);
        }   
    }
}
