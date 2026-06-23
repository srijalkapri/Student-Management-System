using CRUD.Data;
using CRUD.DTOs;
using CRUD.Interfaces;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repositories
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
    }
}
