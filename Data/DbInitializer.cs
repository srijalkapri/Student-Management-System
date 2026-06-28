using BCrypt.Net;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // Check if any SuperAdmin exists
            if (!await context.Users.AnyAsync(u => u.Role == "SuperAdmin"))
            {
                var superAdminUsername = configuration["SuperAdmin:Username"] ?? "superadmin";
                var superAdminPassword = configuration["SuperAdmin:Password"] ?? "SuperAdmin@123";
                var superAdminFullName = configuration["SuperAdmin:FullName"] ?? "Super Administrator";

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(superAdminPassword);

                var superAdmin = new User
                {
                    Username = superAdminUsername,
                    PasswordHash = hashedPassword,
                    FullName = superAdminFullName,
                    Role = "SuperAdmin",
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(superAdmin);
                await context.SaveChangesAsync();
            }
        }
    }
}
