using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PDManagerWeb.Models;
using PDManagerWeb.Models.DTOs;
using PDManagerWeb.Repositories.Interfaces;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace PDManagerWeb.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        public readonly PDManagerContext _context;
        public readonly IMapper _mapper;
        public AccountRepository(PDManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDTO?> CheckUserAsync(AccountAuthDTO account)
        {
            account.Login = account.Login.Trim();
            byte[] hPass = GetPswdHash(account.Password);
            Account? user = await _context.Accounts.Where(a => a.Login == account.Login && !a.IsDeleted && a.PasswordHash == hPass).FirstOrDefaultAsync();
            return user is null ? null: _mapper.Map<AccountDTO>(user);
        }

        public async Task<AccountDTO?> CreateUserAsync(AccountAuthDTO account)
        {
            account.Login = account.Login.Trim();
            byte[] hPass = GetPswdHash(account.Password);
            Account user = new Account() { Login = account.Login, PasswordHash = hPass };
            await _context.Accounts.AddAsync(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch { return null; }
            return _mapper.Map<AccountDTO>(user);
        }

        private byte[] GetPswdHash(string password) => SHA256.HashData(Encoding.UTF8.GetBytes(password));
    }
}
