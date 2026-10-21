using Application.Projects;
using Application.Services;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using FluentResults;

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
            var project = await _createProject.Execute(dto);
            if (project.IsFailed)
            {
                foreach (var error in project.Errors)
                {
                    if (error is OwnerNotFoundError)
                    {
                        return NotFound(error.Message);
                    }
                }

                return BadRequest(project.Errors);
            }
            return Ok(project.Value);
        }


        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProject(Guid projectId)
        {
            var project = await _getProject.Execute(projectId);
            if (project.IsFailed)
            {
                return NotFound(project.Errors);
            }
            return Ok(project.Value);
        }

        [HttpGet("{projectId}/GenerateExcel")]
        public async Task<IActionResult> GenerateProjectExport(Guid projectId)
        {
            var file = await _generateExcelProject.GenerateExcel(projectId);
            return File(file.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"project-{projectId}.xlsx");
        }


        [HttpPut("{projectId}")]
        public async Task<Result<Project>> UpdateProject(Guid projectId, [FromBody] ProjectDto dto)
        {
            var project = await _updateProject.Execute(projectId, dto);
            if (project.IsFailed)
            {
                return Result.Fail(project.Errors);
            }
            return Result.Ok(project.Value);
        }

        [HttpDelete("{projectId}")]
        public async Task<Result> DeleteProject(Guid projectId)
        {
            var success = await _deleteProject.Execute(projectId);
            if (success.IsFailed)
            {
                return Result.Fail(success.Errors);
            }
            return Result.Ok();
           

        }
    }
}



