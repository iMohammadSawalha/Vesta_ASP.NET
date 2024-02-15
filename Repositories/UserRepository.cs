using Vesta.Data;
using Vesta.DataTransferObjects.User;
using Vesta.Helpers;
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
        public async Task<int> CreateUserAsync(CreateUserRequestDTO register_params)
        {
            try{
            var isEmailAlreadyUsed = await _context.Users.FindAsync(register_params.Email);
                if(isEmailAlreadyUsed != null)
                    return 409;
            var newUser = new User()
            {
                Email = register_params.Email,
                HashedPassword = BCryptHelper.HashPassword(register_params.Password),
                Image = register_params.Image
            };
            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();
            return 200;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 500;
            }
        }
    }
}