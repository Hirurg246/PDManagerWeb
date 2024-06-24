using Microsoft.AspNetCore.Mvc;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories;
using PDManagerWeb.Repositories.Interfaces;
using PDManagerWeb.Services.Interfaces;

namespace PDManagerWeb.Services
{
    public class AccountsQueryService : IAccountsQueryService
    {
        private readonly IAccountsRepository _accountsRepository;
        public AccountsQueryService(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<(IActionResult resJSON, int id)> CheckUserAsync(AccountAuthDTO authDTO)
        {
            if (string.IsNullOrWhiteSpace(authDTO.Login) || string.IsNullOrWhiteSpace(authDTO.Password))
                return (new JsonResult(new { result = 0, message = "Логин и пароль не могут быть пустыми!" }), -1);
            AccountDTO? accountDTO = await _accountsRepository.CheckUserAsync(authDTO);
            if (accountDTO is null) return (new JsonResult(new { result = 0, message = "Неверные логин или пароль, проверьте ввод!" }), 0);
            return (new JsonResult(new { result = 1 }), accountDTO.Id);
        }
    }
}
