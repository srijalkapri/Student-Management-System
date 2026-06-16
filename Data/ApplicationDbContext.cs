using Microsoft.EntityFrameworkCore;
using CRUD.Models;
using System.Security.Cryptography.X509Certificates;
namespace CRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
         
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
