using ShoppingCart.Interface;
using ShoppingCart.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShoppingCart.Api.Controllers
{
    public class OrderController : ApiController
    {
        IOrder _orderService;
        public OrderController(IOrder orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateOrder(OrderModel order)
        {
            ResponseModel response = await _orderService.AddOrUpdateOrder(order);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpPost]
        public async Task<HttpResponseMessage> PlaceOrder(long orderId)
        {
            ResponseModel response = await _orderService.PlaceOrder(orderId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteOrder(OrderModel order)
        {
            ResponseModel response = await _orderService.DeleteOrder(order);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllOrder(long userId)
        {
            ResponseModel response = await _orderService.GetAllOrders(userId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetOrderById(long orderId)
        {
            ResponseModel response = await _orderService.GetOrderById(orderId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
