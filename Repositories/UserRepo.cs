
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

        public User GetUserById(int userId)
        {
            try
            {
                return _context.Users.FirstOrDefault(a => a.UId == userId);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving User with ID {userId}: {ex.Message}");
                throw; // Rethrow the exception if necessary
            }
        }

        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();

        public User GetUSerByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.UEmail == email);
        }


    }
}
