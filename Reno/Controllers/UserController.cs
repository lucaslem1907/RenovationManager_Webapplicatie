using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;
using System.Security.Claims;

namespace Reno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {

        private readonly DatabaseContext db;

        public UserController(DatabaseContext context)
        {
            db = context;
        }
        /*
        [HttpPost("Create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserRegisterDto dto)
        {

            var user = new User(dto.FirstName, dto.LastName, dto.Email, password);
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
        */
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<User>> GetUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var user = await db.Users.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await db.Users.Include(x => x.Projects).ToListAsync();
            return Ok(users);
        }
    }
}
