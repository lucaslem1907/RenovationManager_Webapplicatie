using Application.Subtaks;
using Application.Tasks;
using Domain.Entities;
using Infrastructure;
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
            if (newSubtask == null) { BadRequest("taak niet kunnen aanmaken"); }
            return CreatedAtAction(nameof(CreateSubTask), new { taskItemId = TaskItemId }, new
            {
                Id = newSubtask.Id,
                Title = newSubtask.Title,
                Status = newSubtask.IsCompleted
            });
        }

        [HttpGet("{subtaskId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> getSubtask(Guid subtaskId)
        {
            var subtask = await _getSubtask.getSubTask(subtaskId);
            if (subtask == null) return NotFound("subtaak niet gevonden.");
            return Ok(subtask);
        }

        [HttpPut("{subTaskId}")]
        public async Task<ActionResult<Subtask>> UpdateSubTask(SubTaskDto dto, Guid subTaskId)
        {
            var subtask = await _updateSubtask.Execute(subTaskId, dto);
            if (subtask == null)
            {
                return BadRequest("No Subtask Found");
            }

            return (Ok(subtask));
        }

        [HttpDelete("{subtaskId}")]
        public async Task<ActionResult> DeleteSubTask(Guid subtaskId)
        {
            var subtask = await _deleteSubtask.Execute(subtaskId);
            if (!subtask) return BadRequest("Verwijderen niet gelukt");
            return Ok(new { message = "Verwijderen subtask gelukt"});
        }
    }
}