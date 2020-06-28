using ShoppingCart.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Interface
{
    public interface IProduct
    {
        Task<ResponseModel> GetAllProduct();
        Task<ResponseModel> GetProductById(long productId);
        Task<ResponseModel> GetProductsByCategoryId(long categoryId);
        Task<ResponseModel> GetProductsBySubCategoryId(long subCategoryId);
        Task<ResponseModel> AddOrUpdateProduct(ProductModel productModel);
        Task<ResponseModel> DeleteProduct(ProductModel productModel);
    }
}
