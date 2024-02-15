using Vesta.DataTransferObjects.User;

namespace Vesta.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(CreateUserRequestDTO register_params);
    };
}