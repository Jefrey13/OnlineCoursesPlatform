using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetRolesByUserId(int userId);
    }
}
