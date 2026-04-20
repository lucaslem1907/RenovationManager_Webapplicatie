using Application.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Reno.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUseCase _registerUseCase;
        private readonly LoginUseCase _loginUseCase;

        public AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase)
        {
            _registerUseCase = registerUseCase;
            _loginUseCase = loginUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto RegisterDto)
        {
            var succes = await _registerUseCase.Execute(RegisterDto);
            if (!succes) { return BadRequest("Registeren van user mislukt"); }
            return Ok(new { message = "user geregistreerd" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var (succes, token, user) = await _loginUseCase.Execute(loginDto);
            if (!succes) { return BadRequest("Invalid login or password"); }
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Login,
                    user.Projects
                }
            }

            );

        }
    }
}