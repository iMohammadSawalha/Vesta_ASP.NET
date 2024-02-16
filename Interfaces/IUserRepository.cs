using Vesta.Models;

namespace Vesta.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);

        Task<User?> GetUserByEmailAsync(string userEmail);
    };
}