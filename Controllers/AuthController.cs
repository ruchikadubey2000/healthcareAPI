using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly HealthcareContext _context;


        public AuthController(IConfiguration config,HealthcareContext context)
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public IActionResult Login([FromBody] LoginModel userLogin)
        {
            if (userLogin == null)
                return BadRequest("Invalid Client Request.");

            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(new { Token = token });
            }

            return NotFound("Incorrect Username Or Password, Please Try Again.");
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
                new Claim(ClaimTypes.Gender, user.Gender),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel userLogin)
        {
            var currentuser = _context.User.Where(o => o.UserName.Trim().ToLower() == userLogin.UserName.Trim().ToLower() && o.Password.Trim() == userLogin.Password.Trim());

            if (currentuser != null)
            {
                return currentuser.FirstOrDefault();
            }

            return null;
           
        }
    }
}