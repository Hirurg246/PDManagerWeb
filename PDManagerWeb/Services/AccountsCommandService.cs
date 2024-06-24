using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories;
using PDManagerWeb.Repositories.Interfaces;
using PDManagerWeb.Services.Interfaces;

namespace PDManagerWeb.Services
{
    public class AccountsCommandService :IAccountsCommandService
    {
        private readonly IAccountsRepository _accountsRepository;
        public AccountsCommandService(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<(IActionResult resJSON, int id)> CreateUserAsync(AccountAuthDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return (new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" }), -1);
            AccountDTO? accountDTO = await _accountsRepository.CreateUserAsync(authDTO);
            if (accountDTO is null) return (new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" }), 0);
            return (new JsonResult(new { result = 1 }), accountDTO.Id);
        }
    }
}
