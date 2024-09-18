using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories
{
    public interface IRefreshTokenRepository
    {
        void AddRefreshToken(RefreshToken token);
        RefreshToken GetRefreshToken(string token);
        void RevokeRefreshToken(string token);
        bool SaveChanges();
    }
}
