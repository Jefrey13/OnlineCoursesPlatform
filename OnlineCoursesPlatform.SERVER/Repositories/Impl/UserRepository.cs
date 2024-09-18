using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;
using OnlineCoursesPlatform.SERVER.Repositories;

namespace OnlineCoursesPlatform.SERVER.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener usuario por email con validación de existencia
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new InvalidOperationException($"User with email '{email}' does not exist.");
            }

            return user;
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);  // Obtiene el usuario por ID
        }

        // Agregar nuevo usuario con validación de existencia
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            // Verificar si el usuario ya existe
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            _context.Users.Add(user);
        }

        // Guardar los cambios con validación de errores
        public bool SaveChanges()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes to the database.", ex);
            }
        }
    }
}