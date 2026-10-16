using Application.Subtaks;
using Application.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;


namespace Reno.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/Subtask")]
    public class SubtaskController : ControllerBase
    {


        private readonly GetSubtaskUseCase _getSubtask;
        private readonly UpdateSubtaskUseCase _updateSubtask;
        private readonly CreateSubTaskUseCase _createSubtask;
        private readonly DeleteSubtaskUseCase _deleteSubtask;

        public SubtaskController(GetSubtaskUseCase getSubtask,
            UpdateSubtaskUseCase updateSubtask,
            CreateSubTaskUseCase createSubtask,
            DeleteSubtaskUseCase deleteSubtask)
        {
            _createSubtask = createSubtask;
            _deleteSubtask = deleteSubtask;
            _updateSubtask = updateSubtask;
            _getSubtask = getSubtask;
        }
        [HttpPost("{TaskItemId}")]
        public async Task<ActionResult<Subtask>> CreateSubTask([FromBody] SubTaskDto dto, Guid TaskItemId)
        {
            var newSubtask = await _createSubtask.Execute(TaskItemId, dto);
            if (newSubtask.IsFailed)
            {
                return BadRequest(newSubtask.Errors);
            }
            return CreatedAtAction(nameof(CreateSubTask), new { taskItemId = TaskItemId }, new
            {
                Id = newSubtask.Value.Id,
                Title = newSubtask.Value.Title,
                Status = newSubtask.Value.IsCompleted
            });
        }

        [HttpGet("{subtaskId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> getSubtask(Guid subtaskId)
        {
            var subtask = await _getSubtask.GetSubTask(subtaskId);
            if (subtask.IsFailed)
            {
                return NotFound(subtask.Errors);
            }
            return Ok(subtask.Value);
        }

        [HttpPut("{subTaskId}")]
        public async Task<ActionResult<Subtask>> UpdateSubTask(SubTaskDto dto, Guid subTaskId)
        {
            var subtask = await _updateSubtask.Execute(subTaskId, dto);
            if (subtask.IsFailed)
            {
                return BadRequest(subtask.Errors);
            }

            return Ok(subtask.Value );
        }

        [HttpDelete("{subtaskId}")]
        public async Task<ActionResult> DeleteSubTask(Guid subtaskId)
        {
            var subtask = await _deleteSubtask.Execute(subtaskId);
            if (subtask.IsFailed)
            { return BadRequest(subtask.Errors);}
            return Ok(new { message = "Verwijderen subtask gelukt" });
        }
    }
}