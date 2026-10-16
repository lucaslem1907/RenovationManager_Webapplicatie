using Application.Interfaces;
using Domain.Entities;
using Application.Exceptions;
using FluentResults;

namespace Application.Expenses
{
    public class GetExpenseUseCase
    {
        private readonly IExpenseRepository _repo;

        public GetExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<Result<IEnumerable<Expense>?>> GetExpensesByProjectId(Guid projectId)
        {
            var expenses = await _repo.GetExpensesByProjectId(projectId);
            if(expenses == null)
            {
                return Result.Fail(new ProjectNotFoundError(projectId));
            }
            return Result.Ok(expenses);

        }

        public async Task<Result<IEnumerable<Expense>?>> GetAllExpenses()
        {
            var expenses = await _repo.GetAll();
            return expenses;
        }
    }
}
