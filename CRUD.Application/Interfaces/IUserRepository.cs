using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetById(int id);
        Task<int> Create(User user);
        Task<List<User>> GetPendingUsers();
        Task<User?> Update(User user);
    }
}
