using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentFormatsController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public DocumentFormatsController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var docForms = await _context.DocumentFormats.
                Select(df => new
                {
                    df.Id,
                    df.Extension
                }).
                ToListAsync();
            return new JsonResult(docForms);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] string docFormName)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || string.IsNullOrWhiteSpace(docFormName) || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });

            DocumentFormat docForm = new DocumentFormat() { Extension = docFormName };
            await _context.DocumentFormats.AddAsync(docForm);
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
                docForm.Id,
                docForm.Extension
            });
        }

        [HttpDelete("{docFormId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int docFormId)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            if (user is null || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });
            await _context.DocumentFormats.Where(dt => dt.Id == docFormId).ExecuteDeleteAsync();
            return new JsonResult(new { result = 1 });
        }
    }
}
