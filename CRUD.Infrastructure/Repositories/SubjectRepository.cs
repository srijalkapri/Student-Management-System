using CRUD.Infrastructure.Persistence;
using CRUD.Application.DTOs;
using CRUD.Application.Extensions;
using CRUD.Application.Interfaces;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }

        public async Task<int> UpdateSubject(Subject subject)
        {
            var exists = await _context.Subjects.AnyAsync(s => s.Id == subject.Id);
            if (!exists) return 0;

            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }

        public async Task<int> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return 0;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }

        public async Task<List<SubjectResponseDto>> GetAllSubjects()
        {
            return await _context.Subjects
                .Select(s => new SubjectResponseDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<SubjectResponseDto?> GetSubjectById(int id)
        {
            return await _context.Subjects
                .Where(s => s.Id == id)
                .Select(s => new SubjectResponseDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<SubjectResponseDto>> GetSubjectsPaged(PaginationParameters parameters)
        {
            var query = _context.Subjects
                .OrderBy(s => s.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                var isNumericSearch = int.TryParse(parameters.Search, out var searchId);
                query = query.Where(s =>
                    s.Name.ToLower().Contains(search) ||
                    (isNumericSearch && s.Id == searchId));
            }

            var dtoQuery = query.Select(s => new SubjectResponseDto
            {
                Id = s.Id,
                Name = s.Name
            });

            return await dtoQuery.ToPagedResultAsync(parameters);
        }
    }
}
