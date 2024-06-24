using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromForm] AccountAuthDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" });
            AccountDTO? accountDTO = await _accountRepository.CreateUserAsync(authDTO);
            if (accountDTO is null) return new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" });
            HttpContext.Session.SetInt32("id", accountDTO.Id);
            return new JsonResult(new { result = 1 });
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> AuthUserAsync([FromForm] AccountAuthDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" });
            AccountDTO? accountDTO = await _accountRepository.CheckUserAsync(authDTO);
            if (accountDTO is null) return new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" });
            HttpContext.Session.SetInt32("id", accountDTO.Id);
            return new JsonResult(new { result = 1 });
        }
    }
}
