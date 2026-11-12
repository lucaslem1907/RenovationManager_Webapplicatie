using Application.Administration;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
namespace Reno.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : ControllerBase
    {
        private readonly AdministrationUsecase _adminUseCase;

        public AdministrationController(AdministrationUsecase adminUseCase)
        {
            _adminUseCase = adminUseCase;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var result = await _adminUseCase.ExecuteAssignRole(userId, roleName);

            if (result.IsFailed)
            {
                switch (result.Errors.First())
                {
                    case OwnerNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    case RoleNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    case RoleAlreadyAssignedError:
                        return BadRequest(result.Errors.First().Message);
                    case RoleDeletingError:
                        return BadRequest(result.Errors.First().Message);
                    default:
                        return BadRequest("An error occurred");
                }
            }
            return Ok("Role assigned successfully");
        }

        [HttpPost("delete-role")]
        public async Task<IActionResult> DeleteRole(string userId, string roleName)
        {
            var result = await _adminUseCase.ExecuteDeleteRole(userId, roleName);

            if (result.IsFailed)
            {
                switch (result.Errors.First())
                {
                    case OwnerNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    case RoleNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    case RoleDeletingError:
                        return BadRequest(result.Errors.First().Message);
                    case RoleNotAssignedError:
                        return NotFound(result.Errors.First().Message);
                    default:
                        return BadRequest("An error occurred");
                }
            }
            return Ok("Role deleted successfully");
        }

        [HttpGet("get_full_userDetail")]
        public async Task<ActionResult<List<UserAdministrationDto>>> GetFullUsers([FromQuery] UserAdministrationDto userDto)
        {
            var result = await _adminUseCase.ExecuteGetFullUserDetail(userDto.Id, userDto.Email, userDto.UserName, userDto.FirstName, userDto.LastName);

            if (result.IsFailed)
            {
                switch (result.Errors.First())
                {
                    case OwnerNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    default:
                        return BadRequest("An error occurred");
                }
            }
            return Ok(result.Value);
        }
    }
}
