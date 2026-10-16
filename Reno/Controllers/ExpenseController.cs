using Application.Expenses;
using Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Reno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]

    public class ExpenseController : ControllerBase
    {
        private readonly CreateExpenseUseCase _createExpense;
        private readonly GetExpenseUseCase _getExpense;
        private readonly UpdateExpenseUseCase _updateExpense;
        private readonly DeleteExpenseUseCase _deleteExpense;

        public ExpenseController(
            CreateExpenseUseCase createExpense,
            GetExpenseUseCase getExpense,
            UpdateExpenseUseCase updateExpense,
            DeleteExpenseUseCase deleteExpense)
        {
            _createExpense = createExpense;
            _getExpense = getExpense;
            _updateExpense = updateExpense;
            _deleteExpense = deleteExpense;
        }


        [HttpPost("{projectId}/create")]
        public async Task<ActionResult<Expense?>> CreateExpense(Guid projectId, [FromBody] ExpenseDto dto)
        {

            var expense = await _createExpense.Execute(projectId, dto);
            if (expense.IsFailed)
            {
                return BadRequest(expense.Errors);
            }

            return Ok(expense.Value);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>?>> GetExpenses()
        {

            var expenses = await _getExpense.GetAllExpenses();
            if (expenses.IsFailed)
            {
                return BadRequest(expenses.Errors);
            }
            return Ok(expenses.Value);
        }

        [HttpGet("{projectId}")]
        public async Task<Result<IEnumerable<Expense>>> GetExpensesOfProject(Guid projectId)
        {
            var expenses = await _getExpense.GetExpensesByProjectId(projectId);
            if (expenses.IsFailed)
            {
                return Result.Fail<IEnumerable<Expense>>(expenses.Errors);
            }
            return Result.Ok(expenses.Value);
        }



        [HttpPut("{expenseId}/update")]
        public async Task<Result<Expense>> UpdateExpense(Guid expenseId, [FromBody] ExpenseDto dto)
        {
            var expense = await _updateExpense.Execute(expenseId, dto);
            if (expense.IsFailed)
            {
                return Result.Fail<Expense>(expense.Errors);
            }
            return Result.Ok(expense.Value);
        }

        [HttpDelete("{expenseId}/delete")]
        public async Task<Result> DeleteExpense(Guid expenseId)
        {
            var success = await _deleteExpense.Execute(expenseId);
            if (success.IsFailed)
            {
                return Result.Fail(success.Errors);
            }
            return Result.Ok();
        }
    }
}
