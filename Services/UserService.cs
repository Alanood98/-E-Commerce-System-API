using System;
using System.Linq;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class UserService
    {
        private readonly IUserRepo _userRepo;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepo userRepo, ApplicationDbContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }

        // Add a new user (admin only)
        public void AddNewUser(User currentUser, User newUser)
        {
            if (currentUser.role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can add new users.");
            }

            if (_context.Users.Any(u => u.UEmail == newUser.UEmail))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            newUser.UPassword = HashPassword(newUser.UPassword);
            _userRepo.AddUser(newUser);
        }

        // Delete a user (admin only)
        public void DeleteUser(User currentUser, int userId)
        {
            if (currentUser.role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can delete users.");
            }

            var userToDelete = _context.Users.FirstOrDefault(u => u.UId == userId);
            if (userToDelete == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }

        // Add a new product (admin only)
        public void AddNewProduct(User currentUser, Product product)
        {
            if (currentUser.role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can add new products.");
            }

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // Delete a product (admin only)
        public void DeleteProduct(User currentUser, int productId)
        {
            if (currentUser.role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can delete products.");
            }

            var productToDelete = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (productToDelete == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            _context.Products.Remove(productToDelete);
            _context.SaveChanges();
        }

        // Update product details (admin only)
        public void UpdateProduct(User currentUser, Product product)
        {
            if (currentUser.role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can update products.");
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            _context.SaveChanges();
        }

        // User review functionality (users only, one review per product)
        public void AddReview(User currentUser, int productId, int rating, string comment)
        {
            if (currentUser.role != "User")
            {
                throw new UnauthorizedAccessException("Only users can add reviews.");
            }

            // Ensure the user has purchased the product
            var userOrders = _context.Orders.Where(o => o.UserId == currentUser.UId).SelectMany(o => o.OrderProducts).ToList();
            if (!userOrders.Any(op => op.ProductId == productId))
            {
                throw new InvalidOperationException("You can only review products you have purchased.");
            }

            // Check if the user has already reviewed the product
            if (_context.Reviews.Any(r => r.UId == currentUser.UId && r.ProductId == productId))
            {
                throw new InvalidOperationException("You have already reviewed this product.");
            }

            // Add the review
            var review = new Review
            {
                UId = currentUser.UId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        // Return product (users only)
        public void ReturnProduct(User currentUser, int productId)
        {
            if (currentUser.role != "User")
            {
                throw new UnauthorizedAccessException("Only users can return products.");
            }

            // Logic for returning a product (e.g., restocking inventory, updating order status).
            var orderProduct = _context.OrderProducts.FirstOrDefault(op => op.ProductId == productId && op.Order.UserId == currentUser.UId);
            if (orderProduct == null)
            {
                throw new InvalidOperationException("You cannot return a product you have not purchased.");
            }

            orderProduct.Quantity--;
            _context.SaveChanges();
        }

        // Hash a password securely
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
