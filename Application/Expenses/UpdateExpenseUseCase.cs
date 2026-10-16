using Application.Interfaces;
using Application.Exceptions;
using Application.Expenses;
using Domain.Entities;
using Shared.DTO;

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

        public async Task<Expense?> Execute(Guid expenseId, ExpenseDto dto)
        {
            var expense = await _repo.GetExpenseById(expenseId);
            if (expense == null)
            {
                throw new ExpenseNotFoundException(expenseId);
            }
            var project = await _projectRepo.GetById(expense.ProjectId);
            var projectExpenses = await _repo.GetExpensesByProjectId(project.Id);


            bool budgetCheck = BudgetExceededWithUpdatedAmount(projectExpenses, expense.Amount, dto.Amount, project.Budget, dto.ForceBudget);
            if (budgetCheck)
            {
                throw new BudgetExceededException(project.Budget);
            }

            expense.Name = dto.Name;
            expense.Description = dto.Description;
            expense.RoomId = dto.RoomId;
            expense.Amount = dto.Amount;
            expense.Status = dto.Status;
            expense.CreatedDate = dto.CreatedDate;
            await _repo.SaveChanges(); ;
            return expense;
        }

        public static bool BudgetExceededWithUpdatedAmount(IEnumerable<Expense?> expenses, decimal oldAmount, decimal newAmount, decimal? totalBudget, bool? force)
        {
            if (totalBudget == null) return false;
            if (force == true) return false;

            decimal currentBudget = 0;
            foreach (var item in expenses)
            {
                currentBudget += item?.Amount ?? 0;

            }
            var newBudget = currentBudget - oldAmount + newAmount;

            return newBudget > totalBudget;

        }
    }


}
