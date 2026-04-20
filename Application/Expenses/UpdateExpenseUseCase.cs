using Application.Interfaces;
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
            if (expense == null) { return null; }


            expense.Name = dto.Name;
            expense.Description = dto.Description;
            expense.RoomId = dto.RoomId;
            expense.Amount = dto.Amount;
            expense.Status = dto.Status;
            await _repo.SaveChanges(); ;
            return expense;
        }
    }


}
