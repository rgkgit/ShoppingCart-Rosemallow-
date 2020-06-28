using ShoppingCart.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Interface
{
    public interface IUser
    {
        Task<ResponseModel> GetAllUser();
        Task<ResponseModel> GetUserById(long userId);
        Task<ResponseModel> AddOrUpdateUser(UserModel userModel);
        Task<ResponseModel> DeleteUser(UserModel userModel);
    }
}
