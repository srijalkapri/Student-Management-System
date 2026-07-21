using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task Add(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenHash(string tokenHash);
        Task Update(RefreshToken refreshToken);
        Task RevokeAllActiveForUser(int userId);
    }
}
