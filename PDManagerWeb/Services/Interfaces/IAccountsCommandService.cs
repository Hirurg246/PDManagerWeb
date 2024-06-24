using Microsoft.AspNetCore.Mvc;
using PDManagerWeb.Models.DTOs;

namespace PDManagerWeb.Services.Interfaces
{
    public interface IAccountsCommandService
    {
        public Task<(IActionResult resJSON, int id)> CreateUserAsync(AccountAuthDTO authDTO);
    }
}
