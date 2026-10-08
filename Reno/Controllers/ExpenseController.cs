using Application.Expenses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Application.Exceptions;

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
            try
            {
                var expense = await _createExpense.Execute(projectId, dto);
                return Ok(new
                {
                    expense.Name,
                    expense.Amount,
                    expense.Id,
                    date = expense.CreatedDate,
                    expense.Status
                });
            }
            catch (BaseException ex)
            {
                return HandleBaseException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            try
            {
                var expenses = await _getExpense.GetAllExpenses();
                return Ok(expenses);
            }
            catch (BaseException ex)
            {
                return HandleBaseException(ex);
            }
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesOfProject(Guid projectId)
        {
            try
            {
                var expenses = await _getExpense.GetExpensesByProjectId(projectId);
                return Ok(expenses);
            }
            catch (BaseException ex)
            {
                return HandleBaseException(ex);
            }
        }



        [HttpPut("{expenseId}/update")]
        public async Task<ActionResult> UpdateExpense(Guid expenseId, [FromBody] ExpenseDto dto)
        {
            try
            {
                var expense = await _updateExpense.Execute(expenseId, dto);
                return Ok(expense);
            }
            catch (BaseException ex)
            {
                return HandleBaseException(ex);
            }
        }

        [HttpDelete("{expenseId}/delete")]
        public async Task<ActionResult> DeleteExpense(Guid expenseId)
        {
            try
            {
                var success = await _deleteExpense.Execute(expenseId);
                return NoContent();
            }
            catch (BaseException ex)
            {
                return HandleBaseException(ex);
            }
        }

        protected ActionResult HandleBaseException(BaseException ex)

        {
            return StatusCode(ex.StatusCode, ex.Message);
        }

    }
}
