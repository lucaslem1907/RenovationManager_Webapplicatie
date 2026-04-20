using Application.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Reno.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GetUserProjectsUseCase _GetUser;

        public UserController(GetUserProjectsUseCase getuser)
        {
            _GetUser = getuser;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserProjects(Guid userId)
        {
            var userProjects = await _GetUser.Execute(userId);
            if (userProjects == null) { return BadRequest("geen projecten voor deze user"); }
            return Ok(userProjects);
        }


    }
}