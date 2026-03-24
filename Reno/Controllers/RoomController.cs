using Application.Expenses;
using Application.Rooms;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;



namespace Reno.Controllers

{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {

        private readonly CreateRoomUseCase _createRoom;
        private readonly GetRoomUseCase _getRoom;
        private readonly UpdateRoomUseCase _updateRoom;
        private readonly DeleteRoomUseCase _deleteRoom;

        public RoomController(CreateRoomUseCase createRoom, 
            GetRoomUseCase getRoom, 
            UpdateRoomUseCase updateRoom, 
            DeleteRoomUseCase deleteRoom)
        {
            _createRoom = createRoom;
            _getRoom = getRoom;
            _updateRoom = updateRoom;
            _deleteRoom = deleteRoom;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _getRoom.GetAllRooms();
            if (rooms == null) { return NotFound(); }
            return Ok(rooms);
        }

        [HttpGet("{roomid}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomWithId(Guid roomid)
        {
            var room = await _getRoom.GetRoomWithTaskAndSubTasks(roomid);
            if (room == null || room.Count() == 0) return NotFound("Room niet gevonden.");
            return Ok(room);
        }

        [HttpPost("{projectId}/room/create")]
        public async Task<ActionResult> AddRoom(Guid projectId, [FromBody] RoomDto dto)
        {
            var newRoom = await _createRoom.Execute(projectId, dto);
            if (newRoom == null) return BadRequest("creatie van kamer mislukt");



            return CreatedAtAction(nameof(AddRoom), new { projectId = projectId }, new
            {
                Id = newRoom.Id,
                Name = newRoom.Name,
                Status = newRoom.Status
            });
        }


        [HttpDelete("{roomId}")]
        public async Task<ActionResult> DeleteRoom(Guid roomId, [FromQuery] bool deleteExpenses = false)
        {
            var succes = await _deleteRoom.Execute(roomId, deleteExpenses);
            if (!succes) return NotFound("Room not found.");
                    
            return Ok(new {message = "Room is verwijderd"});
        }
    }
}
