using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories;
using PDManagerWeb.Repositories.Interfaces;
using PDManagerWeb.Services.Interfaces;

namespace PDManagerWeb.Services
{
    public class AccountCommandService :IAccountCommandService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountCommandService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> CreateUserAsync(AccountAuthDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" });
            AccountDTO? accountDTO = await _accountRepository.CreateUserAsync(authDTO);
            if (accountDTO is null) return new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" });
            HttpContext.Session.SetInt32("id", accountDTO.Id);
            return new JsonResult(new { result = 1 });
        }
    }
}
