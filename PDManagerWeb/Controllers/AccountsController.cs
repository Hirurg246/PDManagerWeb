using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.DTOs;
using PDManagerWeb.Models;
using System.Security.Cryptography;
using System.Text;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly PDManagerContext _context;
        public AccountsController(PDManagerContext context)
        {
            _context = context;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromForm] AccountDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" });
            authDTO.Login = authDTO.Login.Trim();
            byte[] hPass = SHA256.HashData(Encoding.UTF8.GetBytes(authDTO.Password));
            Account user = new Account() { Login = authDTO.Login, PasswordHash = hPass };
            await _context.Accounts.AddAsync(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(new { result = 0, message = "Некорректные логин или пароль, проверьте ввод!" });
            }
            HttpContext.Session.SetInt32("id", user.Id);
            return new JsonResult(new { result = 1 });
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> AuthUserAsync([FromForm] AccountDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" });
            authDTO.Login = authDTO.Login.Trim();
            byte[] hPass = SHA256.HashData(Encoding.UTF8.GetBytes(authDTO.Password));
            Account? user = await _context.Accounts.Where(a => a.Login == authDTO.Login && !a.IsDeleted && a.PasswordHash == hPass).FirstOrDefaultAsync();
            if (user is null)
                return new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" });
            HttpContext.Session.SetInt32("id", user.Id);
            return new JsonResult(new { result = 1 });
        }
    }
}
