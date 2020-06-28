using ShoppingCart.Interface;
using ShoppingCart.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShoppingCart.Api.Controllers
{
    public class SubCategoryController : ApiController
    {
        ISubCategory _subCategoryService;
        public SubCategoryController(ISubCategory subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateSubCategory(SubCategoryModel subCategory)
        {
            ResponseModel response = await _subCategoryService.AddOrUpdateSubCategory(subCategory);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteSubCategory(SubCategoryModel subCategory)
        {
            ResponseModel response = await _subCategoryService.DeleteSubCategory(subCategory);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSubCategory(long categoryId)
        {
            ResponseModel response = await _subCategoryService.GetAllSubCategory(categoryId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetSubCategoryById(long subCategoryId)
        {
            ResponseModel response = await _subCategoryService.GetSubCategoryById(subCategoryId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
