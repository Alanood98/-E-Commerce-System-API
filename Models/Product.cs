using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal ProductPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0.")]
        public int Stock { get; set; }
        [Required]
        public decimal OverallRating { get; set; } = 5;

        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
