using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Reno.DTO;

namespace Reno.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public AuthController(DatabaseContext db, PasswordService passwordService, JwtService jwtService)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTO.UserRegisterDto RegisterDto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == RegisterDto.Email))
            {
                return BadRequest("Email already in use.");
            }
            var hash = _passwordService.HashPassword(RegisterDto.Password);
            var user = new User(
                RegisterDto.FirstName,
                RegisterDto.LastName,
                RegisterDto.Email,
                hash);

            _db.Users.Add(user);

            await _db.SaveChangesAsync();
            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (loginDto == null) { return BadRequest("login Emty"); }
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == loginDto.login);
            if (user == null) { return NotFound("Login bestaat niet"); }

            var valid = _passwordService.VerifyPassword(loginDto.password, user.PasswordHash);
            if (!valid) { return BadRequest("Paswoord klopt niet"); }

            var token = _jwtService.GenerateToken(user);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Login
                }
            }

            );

        }
    }
}