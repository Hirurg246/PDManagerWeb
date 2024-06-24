using Microsoft.AspNetCore.Mvc;
using PDManagerWeb.Models.DTOs;

namespace PDManagerWeb.Services.Interfaces
{
    public interface IAccountCommandService
    {
        public Task<IActionResult> CreateUserAsync(AccountAuthDTO authDTO);
    }
}
