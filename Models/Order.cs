using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }



        [ForeignKey("user")]
        public int UId { get; set; }

        [JsonIgnore]
        public virtual User user { get; set; }


        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
