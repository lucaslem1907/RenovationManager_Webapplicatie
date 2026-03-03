using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;



namespace Reno.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {

        public DatabaseContext _db;

        public RoomController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _db.Rooms.ToListAsync();
            if (rooms == null || rooms.Count == 0) { return NotFound(); }
            return Ok(rooms);
        }

        [HttpGet("{projectId}/rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetProjectRooms(Guid projectId)
        {
            var project = await _db.RenovationProjects.Include(p => p.Rooms).FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) return NotFound("Project niet gevonden.");
            return Ok(project.Rooms);
        }

        [HttpPost("{projectId}/room/create")]
        public async Task<ActionResult> AddRoom(Guid projectId, [FromBody] RoomDto dto)
        {
            var project = await _db.RenovationProjects.FindAsync(projectId);
            if (project == null) return NotFound("Project niet gevonden.");

            var newRoom = new Room
            {
                Name = dto.Name,
                ProjectId = project.Id
            };

            _db.Rooms.Add(newRoom);


            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(AddRoom), new { projectId = project.Id }, new
            {
                Id = newRoom.Id,
                Name = newRoom.Name
            });
        }


        [HttpDelete("{projectId}/{roomId}")]
        public async Task<ActionResult> DeleteRoom(Guid projectId, Guid roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null) return NotFound("Project niet gevonden.");

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
