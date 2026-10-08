using Application.Expenses;
using Application.Exceptions;
using Application.Rooms;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var rooms = await _getRoom.GetAllRooms();
                return Ok(rooms);
            }
            catch (BaseException ex)
            {

                return NotFound(new {ex.ErrorCode, ex.Message});
            }
            
        }

        [HttpGet("{roomid}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomWithId(Guid roomid)
        {
            try
            {
                var room = await _getRoom.GetRoomWithTaskAndSubTasks(roomid);
                return Ok(room);
            }
            catch (BaseException ex)
            {
                return NotFound(new {ex.ErrorCode, ex.Message});
            }
            
                        
        }

        [HttpPost("{projectId}/room/create")]
        public async Task<ActionResult> AddRoom(Guid projectId, [FromBody] RoomDto dto)
        {

            try
            {
                var newRoom = await _createRoom.Execute(projectId, dto);
                return CreatedAtAction(nameof(AddRoom), new { projectId = projectId }, new
                {
                    Id = newRoom.Id,
                    Name = newRoom.Name,
                    Note = newRoom.Note,
                    Status = newRoom.Status
                });
            }
            catch (BaseException ex)
            {

               return NotFound(new {ex.ErrorCode, ex.Message});
            }
        }

        [HttpPut("{roomId}")]
        public async Task<ActionResult> UpdateRoom(Guid roomId, [FromBody] RoomDto dto)
        {

            try
            {
                var room = await _updateRoom.Execute(roomId, dto);
                return Ok(room);
            }
            catch (BaseException ex)
            {

                return NotFound(new {ex.ErrorCode, ex.Message});
            }
            
            
            
        }

        [HttpDelete("{roomId}")]
        public async Task<ActionResult> DeleteRoom(Guid roomId, [FromQuery] bool deleteExpenses)
        {
            try
            {
                var succes = await _deleteRoom.Execute(roomId, deleteExpenses);
                return Ok(new { message = "Room has been deleted" });
            }
            catch (BaseException ex)
            {
                return BadRequest(new {ex.ErrorCode, ex.Message});
            }
            
        }
    }
}
