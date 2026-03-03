using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;
namespace Reno.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public ProjectController(DatabaseContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _db.RenovationProjects.ToListAsync();
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProject(Guid projectId)
        {
            var project = await _db.RenovationProjects.FindAsync(projectId);
            if (project == null) return NotFound("Project niet gevonden.");
            return Ok(project);
        }


        [HttpPost("create")]
        public async Task<ActionResult<Project>> CreateProject([FromBody] ProjectDto dto)
        {
            var project = new Project(dto.Name, dto.OwnerId);
            await _db.AddAsync(project);
            await _db.SaveChangesAsync();
            return Ok(project);

        }


        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(Guid projectId)
        {
            var project = await _db.RenovationProjects.FindAsync(projectId);
            if (project == null) return NotFound("Project niet gevonden.");

            _db.RenovationProjects.Remove(project);
            await _db.SaveChangesAsync();

            return NoContent();
        }




    }
}



