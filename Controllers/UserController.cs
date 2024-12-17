using System;
using System.Collections.Generic;
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

        [AllowAnonymous]
        [HttpPost("add")]
        public IActionResult AddUser([FromBody] UserInput newUserInput)
        {
            try
            {
                var newUser = new User
                {
                    UName = newUserInput.UName,
                    UEmail = newUserInput.UEmail,
                    UPassword = newUserInput.UPassword,
                    CreatedAt = newUserInput.CreatedAt,
                    UPhone = newUserInput.UPhone,
                    role = newUserInput.Role
                };

                _userService.AddUser(newUser);
                return Ok(new { UserId = newUser.UId, Message = "User added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string email, string password)
        {
            var user = _userService.GetUser(email, password);

            if (user != null)
            {
                string token = GenerateJwtToken(user.UId.ToString(), user.UName);
                return Ok(new { Token = token, Message = "Login successful." });
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        [NonAction]
        public string GenerateJwtToken(string userId, string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("makeorder")]
        public IActionResult MakeOrder([FromBody] Order order)
        {
            var currentUser = HttpContext.Items["User"] as User;
            _userService.MakeOrder(currentUser, order);
            return Ok(new { Message = "Order created successfully." });
        }

        [HttpPost("returnproduct/{productId}")]
        public IActionResult ReturnProduct(int productId)
        {
            var currentUser = HttpContext.Items["User"] as User;
            _userService.ReturnProduct(currentUser, productId);
            return Ok(new { Message = "Product returned successfully." });
        }
    }
}
