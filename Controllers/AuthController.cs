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

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
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

            return NotFound("Incorrect Username Or Password,Please Try Again.");
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
            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

            //if (currentUser != null)
            //{
            //    return currentUser;
            //}

            // return null;
            if(userLogin.UserName == "rudubey"&& userLogin.Password=="123lola123")
                    {
                return new UserModel
                {
                    UserName = "rudubey",
                    DateOfBirth = DateTime.ParseExact("2000-02-03 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                                            System.Globalization.CultureInfo.InvariantCulture),
                    Gender = "Female",
                    Password = "123lola123",
                    FullName = "Ruchika Dubey",
                    Role = "Admin",
                    Email = "ruchikadubey2000@gmail.com"


                };
            }
            return null;
        }
    }
}