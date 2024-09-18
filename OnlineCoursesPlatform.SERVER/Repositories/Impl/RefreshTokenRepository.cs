using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories.Impl
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddRefreshToken(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return _context.RefreshTokens.FirstOrDefault(rt => rt.Token == token && !rt.IsRevoked);
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
            }
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
