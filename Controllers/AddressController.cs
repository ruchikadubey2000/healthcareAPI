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
    public class AddressController : ControllerBase
    {
        private readonly HealthcareContext _context;

        public AddressController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: GetAllAddressByUserName
        [HttpGet("GetAllAddressByUserName/{userName}")]
        public async Task<ActionResult<IEnumerable<AddressModel>>> GetAllAddressByUserName(string userName)
        {
            return await _context.Address.Where(p=>p.User.UserName == userName).ToListAsync();
        }

        // PUT: UpdateAddress
        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(AddressModel addressModel)
        {         
            _context.Entry(addressModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressModelExists(addressModel.Id))
                {
                    return NotFound($"Could Not Update Details For The Address With Name {addressModel.Name},Please Try Again.");
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        // POST:AddNewAddress
        [HttpPost("AddNewAddress")]
        public async Task<ActionResult> AddNewAddress(AddAddressModel address)
        {
            var userModel = await _context.User.Where(p => p.UserName == address.UserName).ToListAsync();
            var addressModel = new AddressModel()
            {
                Name = address.Name,
                Address = address.Address,
                City = address.City,
                State = address.State,
                PhoneNo = address.Address,
                PinCode = address.Address,
                User = userModel.FirstOrDefault(),
            };
            
            _context.Address.Add(addressModel);
            await _context.SaveChangesAsync();

            return Ok();
         }

        // DELETE: DeleteAddressById
        [HttpDelete("DeleteAddressById/{id}")]
        public async Task<ActionResult> DeleteAddressById(int id)
        {
            var addressModel = await _context.Address.FindAsync(id);
            if (addressModel == null)
            {
                return NotFound($"Could Not Delete Address With Name { addressModel.Name},Please Try Again.");
            }

            _context.Address.Remove(addressModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AddressModelExists(int id)
        {
            return _context.Address.Any(e => e.Id == id);
        }
    }
}
