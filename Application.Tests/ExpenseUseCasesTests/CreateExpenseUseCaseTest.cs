using Application.Expenses;
using Application.Interfaces;
using Moq;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;
using FluentResults;

namespace Application.Tests.ExpenseUseCasesTests
{
    public class CreateExpenseUseCaseTest
    {
        [Fact]
        public async Task CreateExpense_ShouldReturnExpenseId_WhenExpenseIsCreated()
        {
            //Arange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var mockRoomRepository = new Mock<IRoomRepository>();
            var createExpenseUseCase = new CreateExpenseUseCase(mockExpenseRepository.Object, mockProjectRepository.Object, mockRoomRepository.Object);


            var project = new ProjectBuilder().WithBudget(1000).Build();
            var room = new RoomBuilder().WithProjectId(project.Id).Build();

            var expenseDto = new ExpenseDtoBuilder()
                .WithRoomId(room.Id)
                .WithDescription("Test Description")
                .WithStatus(ExpenseStatus.unpaid)
                .WithAmount(100)
                .Build();

            //Setup 
            mockProjectRepository.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);
            mockRoomRepository.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);

            //Act
            var result = await createExpenseUseCase.Execute(project.Id, expenseDto);

            //Assert
            Assert.True(result.IsSuccess);

            var createdExpense = result.ValueOrDefault;
            Assert.NotNull(createdExpense);
            Assert.Equal(expenseDto.Amount, createdExpense!.Amount);
            Assert.Equal(expenseDto.Name, createdExpense.Name);
            Assert.Equal(expenseDto.RoomId, createdExpense.RoomId);
            Assert.Equal(expenseDto.Description, createdExpense.Description);

            mockExpenseRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Expense>()), Times.Once);
            mockExpenseRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateExpense_ShouldThrowProjectNotFoundError_WhenProjectDoesNotExist()
        {
            //Arange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var mockRoomRepository = new Mock<IRoomRepository>();
            var createExpenseUseCase = new CreateExpenseUseCase(mockExpenseRepository.Object, mockProjectRepository.Object, mockRoomRepository.Object);
            var expenseDto = new ExpenseDtoBuilder().Build();
            var nonExistentProjectId = Guid.NewGuid();


            //Setup 
            mockProjectRepository.Setup(repo => repo.GetById(nonExistentProjectId)).ReturnsAsync((Domain.Entities.Project?)null);
            //Act
            var result = await createExpenseUseCase.Execute(nonExistentProjectId, expenseDto);
            //Assert
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task CreateExpense_ShouldReturnExpense_WhenForceBudgetisTrue()
        {
            //Arange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var mockRoomRepository = new Mock<IRoomRepository>();
            var createExpenseUseCase = new CreateExpenseUseCase(mockExpenseRepository.Object, mockProjectRepository.Object, mockRoomRepository.Object);

            var project = new ProjectBuilder().WithBudget(100).Build();
            var room = new RoomBuilder().WithProjectId(project.Id).Build();
            var expenseDto = new ExpenseDtoBuilder().WithAmount(200).WithRoomId(room.Id).WithForceBudget(true).Build();
            //Setup 
            mockProjectRepository.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);
            mockRoomRepository.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);
            //Act
            var result = await createExpenseUseCase.Execute(project.Id, expenseDto);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            var createdExpense = result.ValueOrDefault;
            Assert.NotNull(createdExpense);

            Assert.Equal(expenseDto.Amount, createdExpense!.Amount);
            Assert.Equal(expenseDto.Name, createdExpense.Name);
            Assert.Equal(expenseDto.RoomId, createdExpense.RoomId);

            mockExpenseRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Expense>()), Times.Once);
            mockExpenseRepository.Verify(repo => repo.SaveChanges(), Times.Once);

        }
    }
}
