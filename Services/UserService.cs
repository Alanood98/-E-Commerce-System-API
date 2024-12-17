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

        // Add a new user (admin only)
        public void AddUser(User newUser)
        {
            if (_context.Users.Any(u => u.UEmail == newUser.UEmail))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            newUser.UPassword = HashPassword(newUser.UPassword);
            _userRepo.AddUser(newUser);
        }


        //==============================================================================================

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
        //============================================================================================
        public User GetUser(string email, string password)
        {
            return _userRepo.GetUSer(email, password);

        }



        //========================================================================================

        // Add a new product (admin only)
        //public void AddNewProduct(User currentUser, Product product)
        //{
        //    if (currentUser.role != "Admin")
        //    {
        //        throw new UnauthorizedAccessException("Only admins can add new products.");
        //    }

        //    _context.Products.Add(product);
        //    _context.SaveChanges();
        //}

        ////=====================================================================================

        //// Delete a product (admin only)
        //public void DeleteProduct(User currentUser, int productId)
        //{
        //    if (currentUser.role != "Admin")
        //    {
        //        throw new UnauthorizedAccessException("Only admins can delete products.");
        //    }

        //    var productToDelete = _context.Products.FirstOrDefault(p => p.ProductId == productId);
        //    if (productToDelete == null)
        //    {
        //        throw new InvalidOperationException("Product not found.");
        //    }

        //    _context.Products.Remove(productToDelete);
        //    _context.SaveChanges();
        //}
        ////===============================================================================

        //// Update product details (admin only)
        //public void UpdateProduct(User currentUser, Product product)
        //{
        //    if (currentUser.role != "Admin")
        //    {
        //        throw new UnauthorizedAccessException("Only admins can update products.");
        //    }

        //    var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
        //    if (existingProduct == null)
        //    {
        //        throw new InvalidOperationException("Product not found.");
        //    }

        //    existingProduct.ProductName = product.ProductName;
        //    existingProduct.ProductPrice = product.ProductPrice;
        //    existingProduct.Stock = product.Stock;

        //    _context.SaveChanges();
        //}
        ////=========================================================================================

        //// User review functionality (users only, one review per product)
        //public void AddReview(User currentUser, int productId, int rating, string comment)
        //{
        //    if (currentUser.role != "User")
        //    {
        //        throw new UnauthorizedAccessException("Only users can add reviews.");
        //    }

        //    // Ensure the user has purchased the product
        //    var userOrders = _context.Orders.Where(o => o.UId == currentUser.UId).SelectMany(o => o.OrderProducts).ToList();
        //    if (!userOrders.Any(op => op.ProductId == productId))
        //    {
        //        throw new InvalidOperationException("You can only review products you have purchased.");
        //    }

        //    // Check if the user has already reviewed the product
        //    if (_context.Reviews.Any(r => r.UId == currentUser.UId && r.ProductId == productId))
        //    {
        //        throw new InvalidOperationException("You have already reviewed this product.");
        //    }

        //    // Add the review
        //    var review = new Review
        //    {
        //        UId = currentUser.UId,
        //        ProductId = productId,
        //        Rating = rating,
        //        Comment = comment,
        //        ReviewDate = DateTime.Now
        //    };

        //    _context.Reviews.Add(review);
        //    _context.SaveChanges();
        //}
        ////================================================================================================

        // Return product (users only)
        public void ReturnProduct(User currentUser, int productId)
        {
            if (currentUser == null || currentUser.role != "User")
            {
                throw new UnauthorizedAccessException("Only users can return products.");
            }

            // Find the user's order that contains the product
            var orderProduct = _context.OrderProducts
                .FirstOrDefault(op => op.ProductId == productId && op.order.UId == currentUser.UId);

            if (orderProduct == null)
            {
                throw new InvalidOperationException("You cannot return a product you have not purchased.");
            }

            // Increase product stock and update quantity in the order
            var product = _context.Products.First(p => p.ProductId == productId);
            product.Stock += orderProduct.Quantity;

            // Remove the product from the order
            _context.OrderProducts.Remove(orderProduct);
            _context.SaveChanges();
        }
        ////============================================================================================

        // Hash a password securely

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        //=======================================================================================
        // MakeOrder method
        public void MakeOrder(User currentUser, Order orderDetails = null)
        {
            if (currentUser == null || currentUser.role != "User")
            {
                throw new UnauthorizedAccessException("Only registered users can make orders.");
            }

            // Ensure there are items in the order
            if (orderDetails == null || orderDetails.OrderProducts == null || !orderDetails.OrderProducts.Any())
            {
                throw new InvalidOperationException("Order must contain at least one product.");
            }

            // Verify all products exist and have sufficient stock
            foreach (var orderProduct in orderDetails.OrderProducts)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderProduct.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {orderProduct.ProductId} does not exist. Order canceled.");
                }
                if (product.Stock < orderProduct.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.ProductName}. Order canceled.");
                }
            }

            // All checks passed: Proceed to create the order and update stock
            foreach (var orderProduct in orderDetails.OrderProducts)
            {
                var product = _context.Products.First(p => p.ProductId == orderProduct.ProductId);
                product.Stock -= orderProduct.Quantity; // Decrease stock
            }

            // Calculate total amount
            decimal totalAmount = orderDetails.OrderProducts.Sum(op =>
                _context.Products.First(p => p.ProductId == op.ProductId).ProductPrice * op.Quantity);

            // Create a new order
            var order = new Order
            {
                UId = currentUser.UId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderProducts = orderDetails.OrderProducts
            };

            // Save order to database
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
