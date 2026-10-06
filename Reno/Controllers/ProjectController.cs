using Application.Projects;
using Application.Services;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Reno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly CreateProjectUseCase _createProject;
        private readonly GetProjectUseCase _getProject;
        private readonly UpdateProjectUseCase _updateProject;
        private readonly DeleteProjectUseCase _deleteProject;
        private readonly GenerateProjectExcel _generateExcelProject;


        public ProjectController(
            CreateProjectUseCase createProject,
            GetProjectUseCase getProject,
            UpdateProjectUseCase updateProject,
            DeleteProjectUseCase deleteProject,
            GenerateProjectExcel generateExcelProject)
        {
            _createProject = createProject;
            _getProject = getProject;
            _updateProject = updateProject;
            _deleteProject = deleteProject;
            _generateExcelProject = generateExcelProject;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Project>> CreateProject([FromBody] ProjectDto dto)
        {
            try
            {
                var result = await _createProject.Execute(dto);
                return Ok(result);
            }
            catch (OwnerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProject(Guid projectId)
        {
            try
            {
                var project = await _getProject.Execute(projectId);
                return Ok(project);
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{projectId}/GenerateExcel")]
        public async Task<IActionResult> GenerateProjectExport(Guid projectId)
        {
            try
            {
                var file = await _generateExcelProject.GenerateExcel(projectId);
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"project-{projectId}.xlsx");
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{projectId}")]
        public async Task<ActionResult> UpdateProject(Guid projectId, [FromBody] ProjectDto dto)
        {
            try
            {
                var project = await _updateProject.Execute(projectId, dto);
                return Ok(project);
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(Guid projectId)
        {
            try
            {
                var success = await _deleteProject.Execute(projectId);
                return NoContent();
            }
            catch (ProjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }








    }
}



