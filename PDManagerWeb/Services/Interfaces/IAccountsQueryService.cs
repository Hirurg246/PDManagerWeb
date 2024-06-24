using Microsoft.AspNetCore.Mvc;
using PDManagerWeb.Models.DTOs;

namespace PDManagerWeb.Services.Interfaces
{
    public interface IAccountsQueryService
    {
        public Task<(IActionResult resJSON, int id)> CheckUserAsync(AccountAuthDTO authDTO);
    }
}
