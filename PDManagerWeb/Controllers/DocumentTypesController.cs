using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypesController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public DocumentTypesController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var docTypes = await _context.DocumentTypes.
                Select(dt => new
                {
                    dt.Id,
                    dt.Name
                }).
                ToListAsync();
            return new JsonResult(docTypes);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] string docTypeName)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || string.IsNullOrWhiteSpace(docTypeName) || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });

            DocumentType docType = new DocumentType() { Name = docTypeName };
            await _context.DocumentTypes.AddAsync(docType);
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
                docType.Id,
                docType.Name
            });
        }

        [HttpDelete("{docTypeId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int docTypeId)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });
            await _context.DocumentTypes.Where(dt => dt.Id == docTypeId).ExecuteDeleteAsync();
            return new JsonResult(new { result = 1 });
        }
    }
}
