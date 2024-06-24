using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAccessesController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public ProductAccessesController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpPost("RequestAccess")]
        public async Task<IActionResult> RequestAccessAsync([FromForm] int productId)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            Product? product = await _context.Products.FindAsync(productId);
            if (user is null || product is null)
                return new JsonResult(new { result = 0 });

            ProductAccess? productAccess = await _context.ProductAccesses.FindAsync(product.Id, user.Id);
            if (productAccess is not null) return new JsonResult(new { result = 0 });

            productAccess = new ProductAccess() { AccountId = user.Id, ProductId = product.Id, AccessLevelId = 1 };
            await _context.ProductAccesses.AddAsync(productAccess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(new { result = 0 });
            }
            return new JsonResult(new { result = 1 });
        }

        [HttpGet("Active")]
        public async Task<IActionResult> GetActiveAsync()
        {
            var productAccesses = await _context.ProductAccesses.
                Where(pa => !pa.IsGranted && !pa.Account.IsDeleted && !pa.Product.IsDeleted).
                Select(pa => new
                {
                    userId = pa.AccountId,
                    userName = pa.Account.Login,
                    productId = pa.ProductId,
                    productName = pa.Product.Name
                }).
                ToListAsync();
            return new JsonResult(productAccesses);
        }

        [HttpPut("Reply")]
        public async Task<IActionResult> ReplyAccessAsync([FromForm] int userId,
                                                          [FromForm] int productId,
                                                          [FromForm] bool answer)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id")),
                     rUser = await _context.Accounts.FindAsync(userId);
            Product? product = await _context.Products.FindAsync(productId);
            if (user is null || rUser is null || product is null || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });

            ProductAccess? productAccess = await _context.ProductAccesses.FindAsync(product.Id, rUser.Id);
            if (productAccess is null || productAccess.IsGranted)
                return new JsonResult(new { result = 1 });

            if (answer) productAccess.IsGranted = true;
            else _context.ProductAccesses.Remove(productAccess);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(new { result = 0 });
            }
            return new JsonResult(new { result = 1 });
        }
    }
}
