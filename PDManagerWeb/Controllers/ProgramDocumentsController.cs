using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using PDManagerWeb.DTOs;
using PDManagerWeb.Models;
using System.Drawing;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramDocumentsController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public ProgramDocumentsController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpGet("Contents")]
        public async Task<IActionResult> GetContents()
        {
            var products = await _context.Products.
                Where(p => !p.IsDeleted).
                Select(p => new { p.Id, p.Name }).
                ToListAsync();
            var formats = await _context.DocumentTypes.Where(t => t.AllowedFormats.Count > 0).Select(t => new
            {
                t.Name,
                Formats = t.AllowedFormats.Select(af => new { af.Id, af.DocumentFormat.Extension }).ToList()
            }).ToListAsync();
            return new JsonResult(new { products, formats });
        }

        [HttpGet("Status/{productId:int}/{formatId:int}")]
        public async Task<IActionResult> GetStatus([FromRoute] int productId, [FromRoute] int formatId)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            Product? product = await _context.Products.FindAsync(productId);
            AllowedFormat? format = await _context.AllowedFormats.FindAsync(formatId);
            if (user is null || product is null || format is null) return new JsonResult(new { result = -1 });
            if (product.HeadId != user.Id && await _context.SysAdmins.FindAsync(user.Id) is null)
            {
                ProductAccess? productAccess = await _context.ProductAccesses.FindAsync(product.Id, user.Id);
                if (productAccess is null) return new JsonResult(new { result = 1 });
                else if (!productAccess.IsGranted) return new JsonResult(new { result = 0 });
            }
            ProgramDocument? document = await _context.ProgramDocuments.FindAsync(product.Id, format.Id);
            if (document is null) return new JsonResult(new { result = 2, format.DocumentFormat.Extension });

            return new JsonResult(new
            {
                result = 3,
                document.LastChangeDate,
                document.LastChangeUser.Login,
                format.DocumentFormat.Extension
            });
        }

        [HttpGet("File/{productId:int}/{formatId:int}")]
        public async Task<IActionResult> GetFile([FromRoute] int productId, [FromRoute] int formatId)
        {
            Account? user = _context.Accounts.Find(HttpContext.Session.GetInt32("id"));
            Product? product = _context.Products.Find(productId);
            AllowedFormat? format = _context.AllowedFormats.Find(formatId);
            if (user is null || product is null || format is null) return new JsonResult(new { result = 0 });

            if (product.HeadId != user.Id && _context.SysAdmins.Find(user.Id) is null)
            {
                ProductAccess? productAccess = _context.ProductAccesses.Find(product.Id, user.Id);
                if (productAccess is null || !productAccess.IsGranted) return new JsonResult(new { result = 0 });
            }

            ProgramDocument? document = _context.ProgramDocuments.Find(product.Id, format.Id);
            if (document is null) return new JsonResult(new { result = 0 });

            var uplPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", product.Id.ToString(), format.FullName);
            return new JsonResult(System.IO.File.Exists(uplPath) ? new
            {
                result = 1,
                File = File(System.IO.File.ReadAllBytes(uplPath), "application/octet-stream", format.FullName)
            } : new
            {
                result = 0
            });
        }
    }
}
