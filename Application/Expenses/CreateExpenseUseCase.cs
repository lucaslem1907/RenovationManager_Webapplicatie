using Application.Interfaces;
using Shared.DTO;
using Domain.Entities;

namespace Application.Expenses
{
    public class CreateExpenseUseCase
    {
        private readonly IExpenseRepository _repo;
        //private readonly IProjectRepository _projectRepo;

        public CreateExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<Expense?> Execute(Guid ProjectId, ExpenseDto dto)
        {
            var project = _repo.GetExpensesByProjectId(ProjectId);
            if (project == null) { return null; }

            var newExpense = new Expense(dto.Amount, dto.Name, ProjectId, dto.RoomId, dto.Description, dto.Status);
            await _repo.Add(newExpense);
            await _repo.SaveChanges();

            return newExpense;
        }
    }
}
