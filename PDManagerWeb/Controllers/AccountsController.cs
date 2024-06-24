using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories.Interfaces;
using PDManagerWeb.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PDManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(IAccountsQueryService accountsQueryService, IAccountsCommandService accountsCommandService) : ControllerBase
    {
        private readonly IAccountsQueryService _accountsQueryService = accountsQueryService;
        private readonly IAccountsCommandService _accountsCommandService = accountsCommandService;

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromForm] AccountAuthDTO authDTO)
        {
            (IActionResult actionResult, int id) = await _accountsCommandService.CreateUserAsync(authDTO);
            if (id > 0) HttpContext.Session.SetInt32("id", id);
            return actionResult;
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> AuthUserAsync([FromForm] AccountAuthDTO authDTO)
        {
            (IActionResult actionResult, int id) = await _accountsQueryService.CheckUserAsync(authDTO);
            if (id > 0) HttpContext.Session.SetInt32("id", id);
            return actionResult;
        }
    }
}
