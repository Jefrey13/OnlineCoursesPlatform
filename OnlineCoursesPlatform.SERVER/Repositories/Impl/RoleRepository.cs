using Microsoft.Extensions.Caching.Memory;
using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Repositories.Impl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public RoleRepository(IMemoryCache cache, ApplicationDbContext context)
        {
            _cache = cache;
            _context = context;
        }
        public IEnumerable<Role> GetRolesByUserId(int userId)
        {
            var cacheKey = $"userRoles_{userId}";
            if (!_cache.TryGetValue(cacheKey, out List<Role> roles))
            {
                roles = _context.userRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToList();
                _cache.Set(cacheKey, roles, TimeSpan.FromMinutes(30));
            }
            return roles;
        }
    }
}