using Database.Context;
using Database.Data.Models;
using Database.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public interface IUserAccountsService
    {
        public Task<List<UserAccount>> GetUserAccountsAsync();

        /// <summary>
        /// Gets specific user account details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<UserAccount?> GetUserAccountAsync(int userId);

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if user is created, otherwise false</returns>
        public Task<bool> CreateUserAccountAsync(string username, string password);
        public Task<UserAccount?> GetUserAccountsByNameAsync(UserLogin userAccount);
    }

    public class UserAccountsService : IUserAccountsService
    {
        private readonly DatabaseContext _context;
        public UserAccountsService(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<List<UserAccount>> GetUserAccountsAsync()
        {
            return await _context.UserAccounts.ToListAsync();
        }

        public async Task<UserAccount?> GetUserAccountAsync(int userId)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(userAccount => userAccount.Id == userId);
        }

        public async Task<bool> CreateUserAccountAsync(string username, string password)
        {
            if ( string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            bool userNameExists = await _context.UserAccounts.AnyAsync(account => account.UserName == username);
            if (userNameExists)
            {
                return false;
            }

            var newUser = new UserAccount
            {
                UserName = username,
                Password = password,
                Role = "Admin"
            };

            await _context.UserAccounts.AddAsync(newUser);
            await _context.SaveChangesAsync();
                  
            return true;
        }

        public async Task<UserAccount?> GetUserAccountsByNameAsync(UserLogin userAccount)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(
                account => account.UserName.ToLower() == userAccount.Username.ToLower() && 
                account.Password == userAccount.Password );
        }
    }
}