using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<int> CreateAsync(User user);
    }
}
