using ShoppingCart.Interface;
using ShoppingCart.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShoppingCart.Api.Controllers
{
    public class ProductController : ApiController
    {
        IProduct _productService;
        public ProductController(IProduct productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddOrUpdateProduct(ProductModel product)
        {
            ResponseModel response = await _productService.AddOrUpdateProduct(product);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteProduct(ProductModel product)
        {
            ResponseModel response = await _productService.DeleteProduct(product);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllProduct()
        {
            ResponseModel response = await _productService.GetAllProduct();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetProductsByCategoryId(long categoryId)
        {
            ResponseModel response = await _productService.GetProductsByCategoryId(categoryId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductsBySubCategoryId(long subCategoryId)
        {
            ResponseModel response = await _productService.GetProductsBySubCategoryId(subCategoryId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetProductById(long productId)
        {
            ResponseModel response = await _productService.GetProductById(productId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
