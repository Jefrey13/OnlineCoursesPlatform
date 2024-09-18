using BCrypt.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using OnlineCoursesPlatform.DTO.RequestDTO;
using OnlineCoursesPlatform.SERVER.Models;
using OnlineCoursesPlatform.SERVER.Repositories;
using OnlineCoursesPlatform.SERVER.Repositories.Impl;
using OnlineCoursesPlatform.SERVER.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineCoursesPlatform.SERVER.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _config = config;
        }

        public bool ValidateUser(LoginRequestDTO loginDto, out User user)
        {
            user = _userRepository.GetUserByEmail(loginDto.Email);

            if (user != null && VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return true;
            }

            return false;
        }

        // Genera un token JWT con roles y permisos
        public string GenerateToken(User user)
        {
            var roles = _roleRepository.GetRolesByUserId(user.Id).ToList();
            var permissions = new List<Permission>();

            foreach (var role in roles)
            {
                var rolePermissions = _permissionRepository.GetPermissionsByRoleId(role.Id);
                permissions.AddRange(rolePermissions);
            }

            // Claims del usuario con roles y permisos
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            claims.AddRange(permissions.Select(permission => new Claim("permission", permission.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }

        //Refresh Token
        public string GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateTokenString(),
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            _refreshTokenRepository.AddRefreshToken(refreshToken);
            _refreshTokenRepository.SaveChanges();

            return refreshToken.Token;
        }
        public bool ValidateRefreshToken(string refreshToken, out User user)
        {
            var token = _refreshTokenRepository.GetRefreshToken(refreshToken);

            if(token == null || token.ExpiryTime > DateTime.UtcNow)
            {
                user = _userRepository.GetUserById(token.UserId);
                return true;
            }
            user = null;
            return false;
        }
        public void RevokeRefreshToken(string refreshToken)
        {
            _refreshTokenRepository.RevokeRefreshToken(refreshToken);
            _refreshTokenRepository.SaveChanges();
        }
        public string GenerateTokenString()
        {
            var randomNumber = new byte[32];

            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}