using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class SubCategoryModel
    {
        public long SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public long CategoryId { get; set; }
    }
}
