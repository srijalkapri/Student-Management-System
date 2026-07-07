using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IAcessLogRepository
    {
        Task AddAsync(AcessLog log, CancellationToken ct = default);
    }
}
