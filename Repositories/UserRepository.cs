using Vesta.Data;
using Vesta.Interfaces;
using Vesta.Models;

namespace Vesta.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }



        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }

        public async Task<User?> GetUserByEmailAsync(string userEmail)
        {
            var user = await _context.Users.FindAsync(userEmail);
            return user;
        }
    }
}