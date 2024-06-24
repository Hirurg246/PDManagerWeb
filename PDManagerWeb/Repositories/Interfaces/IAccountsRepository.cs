using PDManagerWeb.Models.DTOs;

namespace PDManagerWeb.Repositories.Interfaces
{
    public interface IAccountsRepository
    {
        public Task<AccountDTO?> CreateUserAsync(AccountAuthDTO account);
        public Task<AccountDTO?> CheckUserAsync(AccountAuthDTO account);
    }
}
