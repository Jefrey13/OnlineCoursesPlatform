using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);

        User GetUserById(int userId);
        void AddUser(User user);
        bool SaveChanges();
    }
}
