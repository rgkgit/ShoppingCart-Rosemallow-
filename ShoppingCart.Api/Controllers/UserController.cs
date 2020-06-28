using ShoppingCart.Interface;
using ShoppingCart.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShoppingCart.Api.Controllers
{
    public class UserController : ApiController
    {
        ShoppingCart.Interface.IUser _userService;
        public UserController(ShoppingCart.Interface.IUser userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateUser(UserModel user)
        {
            ResponseModel response = await _userService.AddOrUpdateUser(user);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser(UserModel user)
        {
            ResponseModel response = await _userService.DeleteUser(user);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllUser()
        {
            ResponseModel response = await _userService.GetAllUser();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetUserById(long userId)
        {
            ResponseModel response = await _userService.GetUserById(userId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
