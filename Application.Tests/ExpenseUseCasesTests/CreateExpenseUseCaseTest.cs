
using Moq;
using Xunit;
using Application.Exceptions;
using Shared.Builders.DtoBuilders;
using Shared.Builders.EntityBuilders;
using Application.Expenses;
using Domain.Enums;
using Application.Interfaces;

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


            var project = new ProjectBuilder().Build();
            var room = new RoomBuilder().Build();

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
            Assert.NotNull(result);
            Assert.Equal(expenseDto.Amount, result?.Amount);
            Assert.Equal(expenseDto.Name, result?.Name);
            Assert.Equal(expenseDto.RoomId, result?.RoomId);
            Assert.Equal(expenseDto.Description, result?.Description);

            mockExpenseRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Expense>()), Times.Once);
            mockExpenseRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateExpense_ShouldThrowProjectNotFoundException_WhenProjectDoesNotExist()
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
            //Act & Assert
            await Assert.ThrowsAsync<ProjectNotFoundException>(() => createExpenseUseCase.Execute(nonExistentProjectId, expenseDto));
        }

        [Fact]
        public async Task CreateExpense_ShouldReturnExpense_WhenForceBudgetisTrue()
        {
            //Arange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var mockRoomRepository = new Mock<IRoomRepository>();
            var createExpenseUseCase = new CreateExpenseUseCase(mockExpenseRepository.Object, mockProjectRepository.Object, mockRoomRepository.Object);

            var room = new RoomBuilder().Build();
            var project = new ProjectBuilder().WithBudget(100).Build();
            var expenseDto = new ExpenseDtoBuilder().WithAmount(200).WithRoomId(room.Id).WithForceBudget(true).Build();
            //Setup 
            mockProjectRepository.Setup(repo => repo.GetById(project.Id)).ReturnsAsync(project);
            mockRoomRepository.Setup(repo => repo.GetRoomById(room.Id)).ReturnsAsync(room);
            //Act
            var result = await createExpenseUseCase.Execute(project.Id, expenseDto);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(expenseDto.Amount, result?.Amount);
            Assert.Equal(expenseDto.Name, result?.Name);
            Assert.Equal(expenseDto.RoomId, result?.RoomId);

            mockExpenseRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Expense>()), Times.Once);
            mockExpenseRepository.Verify(repo => repo.SaveChanges(), Times.Once);

        }
    }
}
