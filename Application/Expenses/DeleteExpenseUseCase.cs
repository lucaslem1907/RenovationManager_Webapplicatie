using Application.Interfaces;
using Application.Exceptions;
using FluentResults;

namespace Application.Expenses
{
    public class DeleteExpenseUseCase
    {
        private readonly IExpenseRepository _repo;

        public DeleteExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<Result<bool>> Execute(Guid expenseId)
        {
            var expense = await _repo.GetExpenseById(expenseId);
            if (expense == null)
            {
                return Result.Fail(new ExpenseNotFoundError(expenseId));
            }
            await _repo.Delete(expense);
            await _repo.SaveChanges();
            return Result.Ok(true);

        }
    }
}
