using Application.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Application.Exceptions;
using FluentResults;

namespace Reno.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUseCase _registerUseCase;
        private readonly LoginUseCase _loginUseCase;
        private readonly ResetPasswordUseCase _resetPasswordUseCase;

        public AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase, ResetPasswordUseCase resetPasswordUseCase)
        {
            _registerUseCase = registerUseCase;
            _loginUseCase = loginUseCase;
            _resetPasswordUseCase = resetPasswordUseCase;
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

        [HttpPost("reset-password/{Email}")]
        public async Task<IActionResult> ResetPassword(string Email)
        {
            var result = await _resetPasswordUseCase.SendEmail(Email);
            if (result.IsFailed)
            {
                switch(result.Errors.First())
                {
                    case OwnerNotFoundError:    
                        return NotFound(result.Errors.First().Message);
                    default:
                        return BadRequest("Password reset failed");
                }
            }

            return Ok(new { message = "Password reset email sent successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _resetPasswordUseCase.Execute(resetPasswordDto.Email, resetPasswordDto.NewPassword);
            if (result.IsFailed)
            {
                switch (result.Errors.First())
                {
                    case OwnerNotFoundError:
                        return NotFound(result.Errors.First().Message);
                    default:
                        return BadRequest("Password reset failed");
                }
            }
            return Ok(new { message = "Password reset successfully" });
        }
    }
}