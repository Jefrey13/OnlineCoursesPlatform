using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.SERVER.Models;
using System.Data;

namespace OnlineCoursesPlatform.SERVER.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> rolePermissions { get; set; }
        public DbSet<UserRole> userRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
