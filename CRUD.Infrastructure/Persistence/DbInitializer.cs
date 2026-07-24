using BCrypt.Net;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CRUD.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            await context.Database.MigrateAsync();

            // Check if any SuperAdmin exists
            var superAdmin = await context.Users.FirstOrDefaultAsync(u => u.Role == "SuperAdmin");
            if (superAdmin == null)
            {
                var superAdminUsername = configuration["SuperAdmin:Username"] ?? "superadmin";
                var superAdminPassword = configuration["SuperAdmin:Password"] ?? "SuperAdmin@123";
                var superAdminFullName = configuration["SuperAdmin:FullName"] ?? "Super Administrator";

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(superAdminPassword);

                superAdmin = new User
                {
                    Username = superAdminUsername,
                    PasswordHash = hashedPassword,
                    FullName = superAdminFullName,
                    Role = "SuperAdmin",
                    Status = UserStatus.Approved,
                    ApprovedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(superAdmin);
                await context.SaveChangesAsync();
            }
            else
            {
                // Ensure existing SuperAdmin is approved
                if (superAdmin.Status != UserStatus.Approved)
                {
                    superAdmin.Status = UserStatus.Approved;
                    superAdmin.ApprovedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
