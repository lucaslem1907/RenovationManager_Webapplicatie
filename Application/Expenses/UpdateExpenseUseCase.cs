using Application.Interfaces;
using Application.Exceptions;
using Application.Expenses;
using Domain.Entities;
using Shared.DTO;
using FluentResults;

namespace Application.Expenses
{
    public class UpdateExpenseUseCase
    {
        private readonly IExpenseRepository _repo;
        private readonly IProjectRepository _projectRepo;

        public UpdateExpenseUseCase(IExpenseRepository repo, IProjectRepository projectRepo)
        {
            _repo = repo;
            _projectRepo = projectRepo;

        }

        public async Task<Result<Expense?>> Execute(Guid expenseId, ExpenseDto dto)
        {
            var expense = await _repo.GetExpenseById(expenseId);
            if (expense == null)
            {
                return Result.Fail(new ExpenseNotFoundError(expenseId));
            }

            var project = await _projectRepo.GetById(expense.ProjectId);
            var projectExpenses = await _repo.GetExpensesByProjectId(project.Id);

            var sumExpenses = projectExpenses
                .Where(e => e.Id != expenseId)
                .Sum(e => e.Amount) + dto.Amount;
            bool budgetCheck = Checks.BudgetCheck(sumExpenses, project.Budget, dto.ForceBudget);
            if (budgetCheck)
            {
                return Result.Fail(new BudgetExceededError(project.Budget));
            }

            expense.Name = dto.Name;
            expense.Description = dto.Description;
            expense.RoomId = dto.RoomId;
            expense.Amount = dto.Amount;
            expense.Status = dto.Status;
            expense.CreatedDate = dto.CreatedDate;
            await _repo.SaveChanges();
            return expense;
        }

    }


}
