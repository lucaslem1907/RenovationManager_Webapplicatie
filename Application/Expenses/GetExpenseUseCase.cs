using Application.Interfaces;
using Domain.Entities;

namespace Application.Expenses
{
    public class GetExpenseUseCase
    {
        private readonly IExpenseRepository _repo;

        public GetExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<IEnumerable<Expense>> GetExpensesByProjectId(Guid projectId)
        {
            var expenses = await _repo.GetExpensesByProjectId(projectId);
            if (expenses == null) { return null; }

            return expenses;

        }

        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            var expenses = await _repo.GetAll();
            if (expenses == null) { return null; }
            return expenses;
        }
    }
}
