using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories.Impl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetRolesByUserId(int userId)
        {
            return _context.userRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToList();
        }
    }
}