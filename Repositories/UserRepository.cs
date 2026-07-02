using CRUD.Data;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
