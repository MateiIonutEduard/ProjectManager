using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Services
{
    public interface IAccountService
    {
        Task<Token?> CreateAccountAsync(AccountModel account);
        Task<int> CreateProfileAsync(string token, IFormFile img);
        Task<int> UpdateProfileAsync(string token, IFormFile img);
        Task<Token> RefreshTokenAsync(string token);
        Task<Account> GetAccountAsync(string token);
        Task<Profile> GetProfileAsync(int id);
        Task<(Token?, int)> SignIn(string address, string password);
        Task<int> DeleteProfileAsync(string token);
        Task<bool> SignOut(string token);
    }
}
