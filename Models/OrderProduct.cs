using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using static E_CommerceSystem.Models.OrderProduct;

namespace E_CommerceSystem.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderProduct
    {

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }



        [ForeignKey("order")]
            public int OrderId { get; set; }

            [JsonIgnore]
            public virtual Order order { get; set; }

            [ForeignKey("product")]
            public int ProductId { get; set; }

            [JsonIgnore]
            public virtual Product product { get; set; }
        
    

    
    }
}
