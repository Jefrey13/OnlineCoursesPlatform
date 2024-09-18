using BCrypt.Net;
using DotNetEnv;
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
            try
            {
                user = _userRepository.GetUserByEmail(loginDto.Email);

                if (user != null && VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the user.", ex);
            }
        }

        // Genera un token JWT con roles y permisos
        public string GenerateToken(User user)
        {
            try
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

                // Obtener el JWT_KEY desde el archivo .env
                var jwtKey = Env.GetString("JWT_KEY");
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new Exception("JWT Key is missing in configuration");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: Env.GetString("JWT_ISSUER"),   // Obtener JWT_ISSUER desde el archivo .env
                    audience: Env.GetString("JWT_AUDIENCE"), // Obtener JWT_AUDIENCE desde el archivo .env
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the JWT token.", ex);
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }

        // Refresh Token
        public string GenerateRefreshToken(User user)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the refresh token.", ex);
            }
        }

        public bool ValidateRefreshToken(string refreshToken, out User user)
        {
            try
            {
                var token = _refreshTokenRepository.GetRefreshToken(refreshToken);

                if (token != null && token.ExpiryTime > DateTime.UtcNow)
                {
                    user = _userRepository.GetUserById(token.UserId);
                    return true;
                }
                user = null;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the refresh token.", ex);
            }
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            try
            {
                _refreshTokenRepository.RevokeRefreshToken(refreshToken);
                _refreshTokenRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while revoking the refresh token.", ex);
            }
        }

        public string GenerateTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Registro de un nuevo usuario
        public bool Register(RegisterRequestDTO registerDto, out User user)
        {
            try
            {
                user = null;

                if (_userRepository.GetUserByEmail(registerDto.Email) != null)
                {
                    return false; // Email ya en uso
                }

                // Crear un nuevo usuario
                user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    PasswordHash = HashPassword(registerDto.Password)
                };

                // Guardar el nuevo usuario en la base de datos
                _userRepository.AddUser(user);
                return _userRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during the user registration process.", ex);
            }
        }
    }
}