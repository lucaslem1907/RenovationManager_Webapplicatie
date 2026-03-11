using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using Reno.DTO;


namespace Reno.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/Subtask")]
    public class SubtaskController : ControllerBase
    {

        private readonly DatabaseContext _db;

        public SubtaskController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpPost("{TaskItemId}")]
        public async Task<ActionResult<Subtask>> CreateSubTask([FromBody] SubTaskDto dto, Guid TaskItemId)
        {
            var subtask = new Subtask(dto.Title, taskItemId: TaskItemId, dto.IsCompleted);
            _db.Subtasks.Add(subtask);
            await _db.SaveChangesAsync();
            return Ok(subtask);
        }

        [HttpPut("{subTaskId}")]
        public async Task<ActionResult<Subtask>> UpdateSubTask(SubTaskDto dto, Guid subTaskId)
        {
            var subtask = await _db.Subtasks.FindAsync(subTaskId);
            if (subtask == null)
            {
                return BadRequest("No Subtask Found");
            }
            subtask.UpdateSubtask(
                title: dto.Title,
                status: dto.IsCompleted);

            await _db.SaveChangesAsync();

            return(Ok(subtask));
        }

        [HttpDelete("{subtaskId}")]
        public async Task<ActionResult> DeleteSubTask(Guid subtaskId)
        {
            var subtask = await _db.Subtasks.FindAsync(subtaskId);
            _db.Subtasks.Remove(subtask);
            await _db.SaveChangesAsync();
            return Ok($"Removed Subtask");
        }
    }
}
