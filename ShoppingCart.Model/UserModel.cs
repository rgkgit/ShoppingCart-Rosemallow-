using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class UserModel
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string Email { get; set; }
        public List<AddressModel> UserAddresses { get; set; }
    }
}
