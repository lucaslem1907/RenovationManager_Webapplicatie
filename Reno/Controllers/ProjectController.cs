using Application.Projects;
using Application.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System.Security.Claims;

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
            if (project == null) return NotFound("Project niet gevonden");
            return Ok(project);
        }


        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProject(Guid projectId)
        {
           //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var project = await _getProject.Execute(projectId);
            if (project == null) return NotFound("Project niet gevonden.");
            return Ok(project);
        }

        
        [HttpPut("{projectId}")]

        public async Task<ActionResult> UpdateProject(Guid projectId, [FromBody] ProjectDto dto)
        {
            var project = await _updateProject.Execute(projectId, dto);
            if (project == null) return NotFound("Project niet gevonden."); 
            return Ok(project);
        }
        

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(Guid projectId)
        {
            var success = await _deleteProject.Execute(projectId);
            if (!success) return NotFound("Project niet kunnen verwijderen.");
            return NoContent();
        }

        [HttpGet("{projectId}/GenerateExcel")]
        public async Task<IActionResult> GenerateProjectExport (Guid projectId)
        {
            var file = await _generateExcelProject.GenerateExcel(projectId);
            if (file == null) return NotFound("geen export kunnen maken");

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"project-{projectId}.xlsx");
        }

      


    }
}



