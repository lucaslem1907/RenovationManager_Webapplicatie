using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;
using Domain.Entities;

namespace Reno.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {

        private readonly DatabaseContext db;

        public UserController(DatabaseContext context)
        {
            db = context;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto dto)
        {

            var user = new User(dto.FirstName, dto.LastName, dto.Email);
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet("{UserId}")]
        public async Task<ActionResult<User>> GetUser(Guid UserId)
        {
            var user = await db.Users.Include(x => x.Projects).FirstOrDefaultAsync(x => x.Id == UserId);
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
