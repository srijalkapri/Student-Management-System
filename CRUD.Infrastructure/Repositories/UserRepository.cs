using CRUD.Infrastructure.Persistence;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
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
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<int> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<List<User>> GetPendingUsers()
        {
            return await _context.Users
                .Where(u => u.Status == UserStatus.Pending && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> Update(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;

            existing.PasswordHash = user.PasswordHash;
            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Role = user.Role;
            existing.Status = user.Status;
            existing.ApprovedAt = user.ApprovedAt;
            existing.ApprovedByUserId = user.ApprovedByUserId;
            existing.IsDeleted = user.IsDeleted;
            existing.DeletedAt = user.DeletedAt;
            existing.CreatedAt = user.CreatedAt;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
