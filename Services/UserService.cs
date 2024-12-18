using System;
using System.Linq;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using BCrypt.Net;


namespace E_CommerceSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepo userRepo, ApplicationDbContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }
        //==================================================================================================






      

        public void RegisterUser(User user)
        {
            user.UPassword = BCrypt.Net.BCrypt.HashPassword(user.UPassword);
            _userRepo.AddUser(user);
        }

        public User GetUserById(int id) => _userRepo.GetUserById(id);

        public User Login(string email, string password)
        {
            var user = _userRepo.GetUSerByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.UPassword))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
            return user;
        }

    }
}

