using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.UserDTO;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        // Register a new user
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult RegisterUser([FromBody] UserInput userInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UName = userInput.UName,
                    UEmail = userInput.UEmail,
                    UPassword = userInput.UPassword,
                    CreatedAt = DateTime.UtcNow,
                    UPhone = userInput.UPhone,
                    role = userInput.Role
                };

                _userService.RegisterUser(user);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }



        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInputcs loginInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = _userService.Login(loginInput.UEmail, loginInput.UPassword);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid credentials." });
                }

                // Generate JWT token
                string token = GenerateJwtToken(user.UId.ToString(), user.UName, user.role);

                return Ok(new
                {
                    Token = token,
                    Role = user.role,
                    Message = "Login successful."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }





        [NonAction]
        public string GenerateJwtToken(string userId, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [HttpGet("details")]
        public IActionResult GetUserDetails()
        {
            try
            {
                // Extract userId and other claims from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { Message = "User ID not found in token." });
                }

                int userId = int.Parse(userIdClaim);

                // Fetch user details using the service
                var user = _userService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                // Prepare the response DTO
                var userDetails = new UserDetailsDto
                {
                    UId = user.UId,
                    UName = user.UName,
                    UEmail = user.UEmail,
                    UPhone = user.UPhone,
                    role = user.role
                };

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


    }
}