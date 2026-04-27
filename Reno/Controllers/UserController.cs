using Application.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Reno.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GetUserProjectsUseCase _GetUserProjects;

        public UserController(GetUserProjectsUseCase getuser)
        {
            _GetUserProjects = getuser;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetUserProjects(Guid userId)
        {
            var userProjects = await _GetUserProjects.Execute(userId);
            if (userProjects == null) { return BadRequest("geen projecten voor deze user"); }
            return Ok(userProjects);
        }


    }
}