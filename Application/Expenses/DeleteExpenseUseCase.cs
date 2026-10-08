using Application.Interfaces;
using Application.Exceptions;

namespace Application.Expenses
{
    public class DeleteExpenseUseCase
    {
        private readonly IExpenseRepository _repo;

        public DeleteExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<bool> Execute(Guid expenseId)
        {
            var expense = await _repo.GetExpenseById(expenseId);
            if (expense == null) { throw new ExpenseNotFoundException(expenseId); }
            await _repo.Delete(expense);
            await _repo.SaveChanges();
            return true;

        }
    }
}
