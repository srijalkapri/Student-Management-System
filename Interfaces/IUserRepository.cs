using CRUD.Models;

namespace CRUD.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetById(int id);
        Task<int> Create(User user);
    }
}
