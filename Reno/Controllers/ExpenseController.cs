using Application.Expenses;
using Domain.Entities;
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
        public async Task<ActionResult<Expense>> CreateExpense(Guid projectId, [FromBody] ExpenseDto dto)
        {
            var expense = await _createExpense.Execute(projectId, dto);
            if (expense.IsFailed)
            {
                return BadRequest(expense.Errors.Select(e => e.Message));
            }

            return Ok(new
            {
                description = expense.Value.Name,
                expense.Value.Amount,
                expense.Value.Id,
                date = expense.Value.CreatedDate,
                expense.Value.Status
            });

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {

            var expenses = await _getExpense.GetAllExpenses();
            if (expenses == null) { return NotFound(); }

            return Ok(expenses);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesOfProject(Guid projectId)
        {
            var expenses = await _getExpense.GetExpensesByProjectId(projectId);
            if (expenses == null) { return NotFound(); }

            return Ok(expenses);
        }



        [HttpPut("{expenseId}/update")]
        public async Task<ActionResult> UpdateExpense(Guid expenseId, [FromBody] ExpenseDto dto)
        {
            var expense = await _updateExpense.Execute(expenseId, dto);
            if (expense == null) { return NotFound(); }
            return Ok(expense);
        }

        [HttpDelete("{expenseId}/delete")]
        public async Task<ActionResult> DeleteExpense(Guid expenseId)
        {
            var success = await _deleteExpense.Execute(expenseId);
            if (!success) return NotFound("Expense niet kunnen verwijderen.");
            return NoContent();
        }
    }
}
