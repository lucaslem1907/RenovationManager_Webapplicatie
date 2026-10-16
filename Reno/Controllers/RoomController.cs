using Application.Expenses;
using Application.Exceptions;
using Application.Rooms;
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
            if (rooms.IsFailed)
            {
                return NotFound(rooms.Errors);
            }
            return Ok(rooms.Value);


        }

        [HttpGet("{roomid}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomWithId(Guid roomid)
        {
            var room = await _getRoom.GetRoomWithTaskAndSubTasks(roomid);
            if (room.IsFailed)
            {
                return NotFound(room.Errors);
            }
            return Ok(room.Value);

        }

        [HttpPost("{projectId}/room/create")]
        public async Task<ActionResult> AddRoom(Guid projectId, [FromBody] RoomDto dto)
        {

            var newRoom = await _createRoom.Execute(projectId, dto);
            if (newRoom.IsFailed)
            {
                return BadRequest(newRoom.Errors);
            }
            return CreatedAtAction(nameof(AddRoom), new { projectId = projectId }, new
            {
                Id = newRoom.Value.Id,
                Name = newRoom.Value.Name,
                Note = newRoom.Value.Note,
                Status = newRoom.Value.Status
            });

        }

        [HttpPut("{roomId}")]
        public async Task<ActionResult> UpdateRoom(Guid roomId, [FromBody] RoomDto dto)
        {

            var room = await _updateRoom.Execute(roomId, dto);
            if (room.IsFailed)
            {
                return BadRequest(room.Errors);
            }
            return Ok(room.Value);
        }

        [HttpDelete("{roomId}")]
        public async Task<ActionResult> DeleteRoom(Guid roomId, [FromQuery] bool deleteExpenses)
        {

            var succes = await _deleteRoom.Execute(roomId, deleteExpenses);
            if (succes.IsFailed)
            {
                return BadRequest(succes.Errors);
            }
            return Ok(new { message = "Room has been deleted" });

        }

    }
}
