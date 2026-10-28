using Application.Administration;
using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
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
                    default:
                        return BadRequest("An error occurred");
                }
            }
            return Ok("Role deleted successfully");
        }
    }
}
