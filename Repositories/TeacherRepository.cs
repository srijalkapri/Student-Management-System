using CRUD.Models;
using CRUD.Interfaces;
using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using CRUD.Models;
using CRUD.DTOs;
using CRUD.Interfaces;


namespace CRUD.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
    }


}
