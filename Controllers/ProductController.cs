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
    public class ProductController : ControllerBase
    {
        private readonly HealthcareContext _context;

        public ProductController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: GetAllProduct
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProduct()
        {
            return await _context.Product.Include(p=>p.Category.Name).ToListAsync();
        }

        // GET: GetProductById
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            var productModel = await _context.Product.FindAsync(id);

            if (productModel == null)
            {
                return NotFound($"Could Not Get Details For The Product With Id {id},Please Try Again.");
            }

            return productModel;
        }

        // PUT: UpdateProduct
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductModel productModel)
        {           
            _context.Entry(productModel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(productModel.Id))
                {
                    return NotFound($"Could Not Update Details For The Product With Name {productModel.Name},Please Try Again.");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: AddNewProduct        
        [HttpPost("AddNewProduct")]
        public async Task<ActionResult> AddNewProduct(ProductModel productModel)
        {
            _context.Product.Add(productModel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: DeleteProductById
        [HttpDelete("DeleteProductById/{id}")]
        public async Task<ActionResult>DeleteProductById(int id)
        {
            var productModel = await _context.Product.FindAsync(id);
            if (productModel == null)
            {
                return NotFound($"Could Not Delete Product With Id {id},Please Try Again.");
            }
            _context.Product.Remove(productModel);
            await _context.SaveChangesAsync();

            return Ok();
        }
        // PUT: UpdateProductAvailability
        [HttpPut("UpdateProductAvailability/{id}/{isAvailable}")]
        public async Task<IActionResult> UpdateProductAvailability(int id, bool isAvailable)
        {
            var productModel = await _context.Product.Where(p => p.Id == id).ToListAsync();
            var product = productModel.FirstOrDefault();
            product.IsAvailable = isAvailable;
            _context.Product.Update(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelExists(id))
                {
                    return NotFound($"Could Not Update Product Availability for Product With Name {product.Name},Please Try Again.");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool ProductModelExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
