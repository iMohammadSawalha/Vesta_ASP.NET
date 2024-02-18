using Microsoft.EntityFrameworkCore;
using Vesta.Data;
using Vesta.Interfaces;
using Vesta.Models;

namespace Vesta.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly DataContext _context;

        public UserTokenRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateUserToken(UserToken userToken)
        {
            await _context.UserTokens.AddAsync(userToken);
            _context.SaveChanges();
        }

        public void DeleteUserToken(UserToken userToken)
        {
            _context.UserTokens.Remove(userToken);
            _context.SaveChanges();
        }

        public async Task<UserToken?> GetUserTokenByEmail(string email)
        {
           var existingUserToken = await _context.UserTokens.FirstOrDefaultAsync(ut => ut.UserEmail == email);
           return existingUserToken;
        }

        public async Task<UserToken?> GetUserTokenByUUID(string uuid)
        {
            var existingUserToken = await _context.UserTokens.FindAsync(uuid);
            return existingUserToken;
        }
    }
}