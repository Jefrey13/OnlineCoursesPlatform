using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories
{
    public interface IPermissionRepository
    {
        IEnumerable<Permission> GetPermissionsByRoleId(int roleId);
    }
}
