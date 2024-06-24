using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowedFormatsController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public AllowedFormatsController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpGet("Status/{docTypeId:int}/{fileFormatId:int}")]
        public async Task<IActionResult> GetStatusAsync([FromRoute] int docTypeId, [FromRoute] int fileFormatId)
        {
            DocumentType? documentType = await _context.DocumentTypes.FindAsync(docTypeId);
            DocumentFormat? fileFormat = await _context.DocumentFormats.FindAsync(fileFormatId);
            if (documentType is null) return new JsonResult(new { result = -2 });
            if (fileFormat is null) return new JsonResult(new { result = -1 });
            AllowedFormat? allowedFormat = await _context.AllowedFormats.
                Where(af => af.DocumentTypeId == documentType.Id && af.DocumentFormatId == fileFormat.Id).
                FirstOrDefaultAsync();
            return new JsonResult(new { result = allowedFormat is null ? 0 : 1 });
        }

        [HttpPut("Set")]
        public async Task<IActionResult> SetStatusAsync([FromForm] int docTypeId,
                                                        [FromForm] int fileFormatId,
                                                        [FromForm] bool state)
        {
            Account? user = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32("id"));
            DocumentType? documentType = await _context.DocumentTypes.FindAsync(docTypeId);
            DocumentFormat? fileFormat = await _context.DocumentFormats.FindAsync(fileFormatId);
            if (user is null || documentType is null || fileFormat is null || await _context.SysAdmins.FindAsync(user.Id) is null)
                return new JsonResult(new { result = 0 });

            AllowedFormat? allowedFormat = await _context.AllowedFormats.
                Where(af => af.DocumentTypeId == documentType.Id && af.DocumentFormatId == fileFormat.Id).
                FirstOrDefaultAsync();
            if (allowedFormat is null ^ state) return new JsonResult(new { result = 0 });

            if (state)
            {
                allowedFormat = new AllowedFormat() { DocumentTypeId = documentType.Id, DocumentFormatId = fileFormat.Id };
                await _context.AllowedFormats.AddAsync(allowedFormat);
            }
            else
            {
                _context.AllowedFormats.Remove(allowedFormat);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            { }
            return new JsonResult(new { result = 0 });
        }
    }
}
