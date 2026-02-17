using AuthService.Domain.Models;
using AuthService.Infrastructure.Data;

namespace AuthService.Api.SeedData
{
    public class SeedRole
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
                {
                    new Role { RoleName = "Admin", CreatedBy = 1 },
                    new Role {RoleName ="CBI Officer", CreatedBy =1},
                    new Role {RoleName ="User", CreatedBy=1},
                                
                 };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

        }
    }
}
