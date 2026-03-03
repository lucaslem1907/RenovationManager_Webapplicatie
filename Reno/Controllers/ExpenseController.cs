using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;

namespace Reno.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class ExpenseController : ControllerBase
    {
        public DatabaseContext _db;

        public ExpenseController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {

            var expenses = await _db.Expenses.ToListAsync();
            if (expenses == null) { return NotFound(); }

            return Ok(expenses);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesOfProject(Guid projectId)
        {

            var expenses = await _db.RenovationProjects.FindAsync(projectId);
            if (expenses == null) { return NotFound(); }

            return Ok(expenses);
        }

        [HttpPost("{projectId}/create")]
        public async Task<ActionResult<Expense>> CreateExpense(Guid projectId, [FromBody] ExpenseDto dto)
        {
            var project = await _db.RenovationProjects.FindAsync(projectId);
            if (project == null) { return NotFound(); }

            var newExpense = new Expense(dto.Amount, dto.Name, projectId);
            _db.Expenses.Add(newExpense);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateExpense), new { projectId = project.Id }, new
            {
                Id = newExpense.Id,
                Name = newExpense.Name,
                Amount = newExpense.Amount
            });

        }
    }
}
