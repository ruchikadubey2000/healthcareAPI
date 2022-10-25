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
    public class OrderController : ControllerBase
    {
        private readonly HealthcareContext _context;

        public OrderController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: all Orders
        [HttpGet("GetAllOrder")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrder()
        {
            return await _context.Order.Include(p => p.Status.Name).Include(p=>p.User.FullName).ToListAsync();
        }
            // GET: Get Order By id
            [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            var orderModel = await _context.Order.Where(p=>p.Id==id).Include(p=>p.Status).Include(p=>p.Address).Include(p=>p.User).ToListAsync();

            if (orderModel == null)
            {
                return NotFound($"Could Not Find Details For the Order with Order Id {id},Please Try Again.");
            }

            return Ok(orderModel);
        }

        // PUT: UpdateOrderStatusById
        [HttpPut("UpdateOrderStatusById/{id}/{statusId}")]
        public async Task<IActionResult> UpdateOrderStatusById( int id,int statusId)
        {
            var orderModel = await _context.Order.Where(p => p.Id == id).ToListAsync();
            var statusModel = await _context.Status.Where(p => p.Id == statusId).ToListAsync();
            var order = orderModel.FirstOrDefault();
            order.Status = statusModel.FirstOrDefault();
            _context.Order.Update(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderModelExists(id))
                {
                    return NotFound($"Could Not Update Status For the Order with Order Id {id},Please Try Again.");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: AddNewOrder
        [HttpPost("AddNewOrder")]
        public async Task<ActionResult> AddNewOrder(AddOrderModel orders)
        {
            var addressModel = await _context.Address.Where(p => p.Id == orders.AddressID).Include(p=>p.User).ToListAsync();
            var statusModel = await _context.Status.Where(p => p.Id == orders.StatusId).ToListAsync();
            var orderModel = new OrderModel
            {
                TotalPrice = orders.TotalPrice,
                OrderedDate = orders.OrderedDate,
                User = addressModel.FirstOrDefault().User,
                Address = addressModel.FirstOrDefault(),
                Status = statusModel.FirstOrDefault()
            };
            _context.Order.Add(orderModel);
            foreach (var product in orders.ProductDetails)
            {
                var productModel= await _context.Product.Where(p => p.Id == product.ProductId).ToListAsync();
                var productOrdered = new ProductOrderedModel
                {
                    Product = productModel.FirstOrDefault(),
                    Order = orderModel
                };
                _context.ProductOrdered.Add(productOrdered);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {               
                    return BadRequest($"Could Not Place your Order,Please Try Again.");                               
            }
            return Ok();
        }

        private bool OrderModelExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
