using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Provider.EntityModel
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsOrderPlaced { get; set; }
        public long ShippingAddressId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
    }
}
