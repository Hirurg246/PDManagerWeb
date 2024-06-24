using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public ProductsController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpGet("MainInfo")]
        public async Task<IActionResult> GetMainInfoAsync()
        {
            var products = await _context.Products.
                Where(p => !p.IsDeleted).
                Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.StartDate
                }).
                ToListAsync();
            return new JsonResult(products);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] string productName)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || string.IsNullOrWhiteSpace(productName) || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });

            Product product = new Product() { Name = productName, HeadId = user.Id };
            await _context.Products.AddAsync(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(new { result = 0 });
            }
            return new JsonResult(new
            {
                result = 1,
                product.Id,
                product.Name,
                product.StartDate,
                Head = product.Head.Login
            });
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });
            await _context.Products.Where(p => p.Id == productId).ExecuteDeleteAsync();
            return new JsonResult(new { result = 1 });
        }
    }
}
