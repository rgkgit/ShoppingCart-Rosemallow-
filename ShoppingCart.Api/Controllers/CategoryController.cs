using ShoppingCart.Interface;
using ShoppingCart.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShoppingCart.Api.Controllers
{
    public class CategoryController : ApiController
    {
        private ICategory _categoryService;
        public CategoryController(ICategory categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateCategory(CategoryModel category)
        {
            ResponseModel response = await _categoryService.AddOrUpdateCategory(category);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteCategory(CategoryModel category)
        {
            ResponseModel response = await _categoryService.DeleteCategory(category);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllCategory()
        {
            ResponseModel response = await _categoryService.GetAllCategory();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetCategoryById(long categoryId)
        {
            ResponseModel response = await _categoryService.GetCategoryById(categoryId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
