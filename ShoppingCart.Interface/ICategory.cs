using ShoppingCart.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Interface
{
    public interface ICategory
    {
        Task<ResponseModel> GetAllCategory();
        Task<ResponseModel> GetCategoryById(long categoryId);
        Task<ResponseModel> AddOrUpdateCategory(CategoryModel categoryModel);
        Task<ResponseModel> DeleteCategory(CategoryModel categoryModel);
    }
}
