using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using CRUD.Infrastructure.Persistence;

namespace CRUD.Infrastructure.Repositories
{
    public class AccessLogRepository : IAcessLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AccessLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AcessLog log, CancellationToken ct = default)
        {
            _context.AcessLogs.Add(log);
            await _context.SaveChangesAsync(ct);
        }
    }
}   
