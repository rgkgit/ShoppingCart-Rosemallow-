using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class OrderModel
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsOrderPlaced { get; set; }
        public long ShippingAddressId { get; set; }
        public ProductModel ProductDetail { get; set; }
    }
}
