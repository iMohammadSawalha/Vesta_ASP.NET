using Vesta.Models;

namespace Vesta.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<UserToken?> GetUserTokenByEmail(string email);
        Task<UserToken?> GetUserTokenByUUID(string uuid);
        Task CreateUserToken(UserToken userToken);
        void DeleteUserToken(UserToken userToken);
    }
}