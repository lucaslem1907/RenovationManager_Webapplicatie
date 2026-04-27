using Application.Interfaces;
using Domain.Entities;
using FluentResults;
using Shared.DTO;

namespace Application.Expenses
{
    public class CreateExpenseUseCase
    {
        private readonly IExpenseRepository _repo;
        private readonly IProjectRepository _projectRepo;

        public CreateExpenseUseCase(IExpenseRepository repo, IProjectRepository projectRepo)
        {
            _repo = repo;
            _projectRepo = projectRepo;

        }

        public async Task<Result<Expense?>> Execute(Guid ProjectId, ExpenseDto dto)
        {
            var expenses = await _repo.GetExpensesByProjectId(ProjectId);
            var project = await _projectRepo.GetById(ProjectId);


            bool budget = BudgetOverschreden(expenses, dto.Amount, project.Budget, dto.ForceBudget);
            if (budget) { return Result.Fail("Budget is overschreden"); }

            var newExpense = new Expense(dto.Amount, dto.Name, ProjectId, dto.RoomId, dto.Description, dto.Status);
            await _repo.Add(newExpense);
            await _repo.SaveChanges();

            return Result.Ok(newExpense);
        }

        private bool BudgetOverschreden(IEnumerable<Expense?> expenses, decimal amount, decimal? totalBudget, bool? force)
        {
            if (totalBudget == null) return false;
            if (force == true) return false;

            decimal currentBudget = 0;
            foreach (var item in expenses)
            {
                currentBudget += item?.Amount ?? 0;

            }
            var newBudget = currentBudget + amount;

            return newBudget > totalBudget;

        }
    }
}
