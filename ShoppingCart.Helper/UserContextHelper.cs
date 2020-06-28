using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Helper;
using ShoppingCart.Model;

namespace ShoppingCartHelper
{
    public static class UserContextHelper
    {
        public static UserContextModel Context
        {
            get
            {
                UserContextModel context = new UserContextModel();
                var identity = (ClaimsIdentity)System.Web.HttpContext.Current.User.Identity;
                var hashedId = identity.Claims.Where(x => x.Type == "uuq").First().Value;
                var email = identity.Name;

                context.Email = email;
                context.UserID = Convert.ToInt32(Decryptor.Decrypt(hashedId));
                //var _roles = identity.Claims
                //            .Where(c => c.Type == ClaimTypes.Role)
                //            .Select(c => c.Value).ToArray();

                return context;
            }
        }
    }
}
