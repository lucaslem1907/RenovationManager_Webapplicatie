using Application.Interfaces;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTO;

namespace Application.Expenses
{
    public class UpdateExpenseUseCase
    {
        private readonly IExpenseRepository _repo;

        public UpdateExpenseUseCase(IExpenseRepository repo)
        {
            _repo = repo;

        }

        public async Task<Expense?> Execute(Guid expenseId, ExpenseDto dto)
        {
            var expense = await _repo.GetExpenseById(expenseId);
            if (expense == null)
            {
                throw new ExpenseNotFoundException(expenseId);
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
    }


}
