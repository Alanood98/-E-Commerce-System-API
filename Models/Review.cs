using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace E_CommerceSystem.Models
{
    [PrimaryKey(nameof(UId), nameof(ProductId), nameof(RId))]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

       [Required]
        public DateTime ReviewDate { get; set; } = DateTime.Now;



    [ForeignKey("user")]
    public int UId { get; set; }

    [JsonIgnore]
    public virtual User user { get; set; }

    [ForeignKey("product")]
    public int ProductId { get; set; }

    [JsonIgnore]
    public virtual Product product { get; set; }

}
}
