﻿
using E_CommerceSystem.Repositories;
using E_CommerceSystem;
//using E_CommerceSystem.Controllers;
using E_CommerceSystem.Models;


namespace E_CommerceSystem.Repositories
{
    public class UserRepo : IUserRepo
    {

        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUSer(string email, string password)
        {
            return _context.Users.Where(u => u.UEmail == email & u.UPassword == password).FirstOrDefault();
        }

        public User UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating the user: {ex.Message}");
                throw;
            }
        }
        public User DeleteUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when deleting the user: {ex.Message}");
                throw;
            }
        }

    }
}
