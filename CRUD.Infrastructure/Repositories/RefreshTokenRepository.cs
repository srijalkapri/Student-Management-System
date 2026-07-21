using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenHash(string tokenHash)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }

        public async Task Update(RefreshToken refreshToken)
        {
            var existing = await _context.RefreshTokens.FindAsync(refreshToken.Id);
            if (existing == null)
            {
                return;
            }

            existing.RevokedAtUtc = refreshToken.RevokedAtUtc;
            existing.ReplacedByTokenHash = refreshToken.ReplacedByTokenHash;
            existing.ExpiresAtUtc = refreshToken.ExpiresAtUtc;
            existing.TokenHash = refreshToken.TokenHash;
            existing.UserId = refreshToken.UserId;
            existing.CreatedAtUtc = refreshToken.CreatedAtUtc;

            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllActiveForUser(int userId)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId
                            && x.RevokedAtUtc == null
                            && x.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync();

            if (activeTokens.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            foreach (var token in activeTokens)
            {
                token.RevokedAtUtc = now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
