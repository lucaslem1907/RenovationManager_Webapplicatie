using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;

namespace Reno.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {

        public DatabaseContext _db;

        public TaskController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _db.Tasks.ToListAsync();
            if (tasks == null || tasks.Count == 0) { return NotFound(); }
            return Ok(tasks);
        }

        [HttpGet("{roomId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetRoomTasks(Guid roomId)
        {
            var room = await _db.Rooms.Include(r => r.Tasks).FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null) return NotFound("Room niet gevonden.");
            return Ok(room.Tasks);
        }

        [HttpPost("{roomId}/tasks/create")]
        public async Task<ActionResult> AddTask(Guid roomId, [FromBody] TaskDto dto)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null) return NotFound("Room niet gevonden.");
            var newTask = new TaskItem(dto.Title, dto.Description, roomId);

            _db.Tasks.Add(newTask);

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(AddTask), new { roomId = room.Id }, new
            {
                Id = newTask.Id,
                Name = newTask.Title
            });
        }


        [HttpDelete("{roomId}/tasks/{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid roomId, Guid taskId)
        {
            var task = await _db.Tasks.FindAsync(taskId);
            if (task == null) return NotFound("Task niet gevonden.");
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}


