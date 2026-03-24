using Application.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Reno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {

        private readonly GetTaskUseCase _getTask;
        private readonly UpdateTaskUseCase _updateTask;
        private readonly CreateTaskUseCase _createTask;
        private readonly DeleteTaskUseCase _deleteTask;

        public TaskController(GetTaskUseCase getTask, 
            UpdateTaskUseCase updateTask, 
            CreateTaskUseCase createTask, 
            DeleteTaskUseCase deleteTask)
        {
            _createTask = createTask;
            _deleteTask = deleteTask;
            _updateTask = updateTask;
            _getTask = getTask;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var tasks = await _getTask.GetAllTasks();
            if (tasks == null) { return NotFound(); }
            return Ok(tasks);
        }

        [HttpGet("{roomId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetRoomTasks(Guid roomId)
        {
            var tasks = await _getTask.GetTasksByRoomId(roomId);
            if (tasks == null) return NotFound("Room of taken niet gevonden.");
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid taskId)
        {
            var task = await _getTask.getTask(taskId);
            if (task == null) { return NotFound("Taak niet gevonden"); }
            return Ok(task);
        }


        [HttpPost("{roomId}/tasks/create")]
        public async Task<ActionResult> AddTask(Guid roomId, [FromBody] TaskDto dto)
        {
            var newTask = await _createTask.Execute(roomId, dto);
            if (newTask == null) return NotFound("Room niet gevonden.");

            return CreatedAtAction(nameof(AddTask), new { roomId = roomId }, new
            {
                Id = newTask.Id,
                Title = newTask.Title,
                Status = newTask.IsCompleted
            });
        }

        [HttpPut("{roomId}/tasks/{taskId}")]
         public async Task<ActionResult> UpdateTask(Guid roomId, Guid taskId, [FromBody] TaskDto dto)
        {
            var task = await _updateTask.Execute(taskId, dto);
            if (task == null) return NotFound("Task niet gevonden.");
            return Ok(task); 
        }


        [HttpDelete("{roomId}/tasks/{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid roomId, Guid taskId)
        {
            var task = await _deleteTask.Execute(taskId);
            if (task == null) return NotFound("Task niet gevonden.");
            return Ok( new
            {
                message = "De taak is verwijderd"
            });
        }
    }
}


