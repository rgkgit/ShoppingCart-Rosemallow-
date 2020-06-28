using ShoppingCart.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Interface
{
    public interface IOrder
    {
        Task<ResponseModel> GetAllOrders(long userId);
        Task<ResponseModel> GetOrderById(long orderId);
        Task<ResponseModel> AddOrUpdateOrder(OrderModel orderModel);
        Task<ResponseModel> PlaceOrder(long orderId);
        Task<ResponseModel> DeleteOrder(OrderModel orderModel);
    }
}
