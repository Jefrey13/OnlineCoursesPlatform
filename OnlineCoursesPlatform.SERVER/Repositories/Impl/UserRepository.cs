using OnlineCoursesPlatform.SERVER.Data;
using OnlineCoursesPlatform.SERVER.Models;
using OnlineCoursesPlatform.SERVER.Repositories;
using System;

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
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email cannot be null or empty");
                }

                return _context.Users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra en la consulta
                throw new Exception("An error occurred while retrieving the user by email.", ex);
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra en la consulta
                throw new Exception("An error occurred while retrieving the user by ID.", ex);
            }
        }

        // Agregar nuevo usuario con validación de existencia
        public void AddUser(User user)
        {
            try
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
            catch (Exception ex)
            {
                // Manejar cualquier excepción al agregar el usuario
                throw new Exception("An error occurred while adding the user to the database.", ex);
            }
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
                // Manejar cualquier excepción al guardar los cambios
                throw new Exception("An error occurred while saving changes to the database.", ex);
            }
        }
    }
}