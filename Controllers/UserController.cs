using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HealthcareContext _context;

        public UserController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: GetAllUser
        [HttpGet("GetAllUser")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUser()
        {
            return await _context.User.Where(p=>p.Role == "User").ToListAsync();
        }
        // GET: GetAllAdmin
        [HttpGet("GetAllAdmin")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllAdmin()
        {
            return await _context.User.Where(p => p.Role == "Admin").ToListAsync();
        }

        // GET: GetUserByUserName
        [HttpGet("GetUserByUserName/{userName}")]
        public async Task<ActionResult<UserModel>> GetUserByUserName(string userName)
        {
            var userModel = await _context.User.Where(p=>p.UserName== userName).FirstAsync();

            if (userModel == null)
            {
                return NotFound($"Could Not Get Details For The User With UserName {userName},Please Try Again.");
            }

            return userModel;
        }

        // PUT: UpdateUser       
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserModel userModel)
        {
            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(userModel.Id))
                {
                    return NotFound($"Could Not Update Details For The User With Name {userModel.FullName},Please Try Again.");
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        // POST: AddNewUser
        [HttpPost("AddNewUser")]
        public async Task<ActionResult> AddNewUser(UserModel userModel)
        {
            userModel.Role = "User";
            _context.User.Add(userModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: DeleteUserById
        [HttpDelete("DeleteUserById/{id}")]
        public async Task<ActionResult<UserModel>> DeleteUserById(int id)
        {
            var userModel = await _context.User.FindAsync(id);
            if (userModel == null)
            {
                return NotFound($"Could Not Delete User With Id {id},Please Try Again.");
            }
            _context.User.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        private bool UserModelExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
