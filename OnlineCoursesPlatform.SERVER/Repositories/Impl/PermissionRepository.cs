using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories.Impl
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Permission> GetPermissionsByRoleId(int roleId)
        {
            return _context.rolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToList();
        }
    }
}