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
    public class AccountsRepository: IAccountsRepository
    {
        public readonly PDManagerContext _context;
        public readonly IMapper _mapper;
        public AccountsRepository(PDManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDTO?> CheckUserAsync(AccountAuthDTO authDTO)
        {
            Account? user = _mapper.Map<Account>(authDTO);
            user = await _context.Accounts.Where(a => a.Login == user.Login && !a.IsDeleted && a.PasswordHash == user.PasswordHash).FirstOrDefaultAsync();
            return user is null ? null: _mapper.Map<AccountDTO>(user);
        }

        public async Task<AccountDTO?> CreateUserAsync(AccountAuthDTO authDTO)
        {
            Account user = _mapper.Map<Account>(authDTO);
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
