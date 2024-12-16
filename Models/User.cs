using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace E_CommerceSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int UId { get; set; }
        [Required]
        public string UName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9_.-]*@[a-zA-Z0-9-]+(\.com|\.om|\.gov|\.hotmail)$",
         ErrorMessage = "Email must be in the format avcf2@gmail.com, avcf2@gmail.om, avcf2@gmail.gov, or avcf2@gmail.hotmail")]
        
        public string UEmail { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
         ErrorMessage = "Password must include at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.")]
        public string UPassword { get; set; }

        [Required]

        // Automatically sets the registration time to the current time
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UPhone { get; set; }

       
  
        [Required]
        public string role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
