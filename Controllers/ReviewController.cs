using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models;
using System;
using System.Linq;
using E_CommerceSystem.UserDTO;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IReviewRepo _reviewRepo;
        public ReviewController(IReviewService reviewService ,IReviewRepo reviewRepo)
        {
            _reviewService = reviewService;
            _reviewRepo = reviewRepo;
        }

        [HttpPost]
       
        public IActionResult AddReview( ReviewInput reviewInput)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                ;

                var review = new Review
                {
                   
                    Rating = reviewInput.Rating,
                    Comment = reviewInput.Comment,
                    ProductId = reviewInput.ProductId,
                    ReviewDate = reviewInput.ReviewDate,
                    UId = userId
                };

                _reviewService.AddReview(review, userId);
                return Ok(new { Message = "Review added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("product/{productId}")]
        public IActionResult GetAllReviewsForProduct(int productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var reviews = _reviewService.GetAllReviewsForProduct(productId, page, pageSize);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "User")]

        public IActionResult UpdateReview([FromBody] ReviewInput reviewInput)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

                var review = new Review
                {
                    RId = reviewInput.RId,
                    Rating = reviewInput.Rating,
                    Comment = reviewInput.Comment,
                    ProductId = reviewInput.ProductId,
                    ReviewDate = reviewInput.ReviewDate
                };

                _reviewService.UpdateReview(review, userId);
                return Ok(new { Message = "Review updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                _reviewService.DeleteReview(reviewId, userId, role);
                return Ok(new { Message = "Review deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}