using Application.Tasks;
using Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (tasks.IsFailed)
            {
                return BadRequest(tasks.Errors);
            }
            return Ok(tasks.Value);
        }

        [HttpGet("{roomId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetRoomTasks(Guid roomId)
        {
            var tasks = await _getTask.GetTasksByRoomId(roomId);
            if (tasks.IsFailed)
            {
                return BadRequest(tasks.Errors);
            }
            return Ok(tasks.Value);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid taskId)
        {
            var task = await _getTask.getTask(taskId);
            if (task.IsFailed)
            {
                return BadRequest(task.Errors);
            }
            return Ok(task.Value);
        }


        [HttpPost("{roomId}/tasks/create")]
        public async Task<ActionResult> AddTask(Guid roomId, [FromBody] TaskDto dto)
        {
            var newTask = await _createTask.Execute(roomId, dto);
            if (newTask.IsFailed)
            {
                return BadRequest(newTask.Errors);
            }

            return CreatedAtAction(nameof(AddTask), new { roomId = roomId }, new
            {
                Id = newTask.Value.Id,
                Title = newTask.Value.Title,
                Status = newTask.Value.IsCompleted,
                RoomStatus = newTask.Value.Room.Status
            });
        }


        [HttpPut("{roomId}/tasks/{taskId}")]
        public async Task<ActionResult> UpdateTask(Guid roomId, Guid taskId, [FromBody] TaskDto dto)
        {
            var task = await _updateTask.Execute(taskId, dto);
            if (task.IsFailed)
            {
                return BadRequest(task.Errors);
            }
            return Ok(task.Value);
        }

        [HttpDelete("{roomId}/tasks/{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid roomId, Guid taskId)
        {
            var task = await _deleteTask.Execute(taskId);
            if (task.IsFailed)
            {
                return BadRequest(task.Errors);
            }
            return Ok(new
            {
                message = "De taak is verwijderd"
            });
        }
    }
}


