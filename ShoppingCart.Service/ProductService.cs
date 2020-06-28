using AutoMapper;
using ShoppingCart.Interface;
using ShoppingCart.Model;
using ShoppingCart.Provider.EntityModel;
using ShoppingCart.Provider.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Service
{
    public class ProductService : IProduct
    {
        UnitOfWork _unitOfWork;
        public ProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> AddOrUpdateProduct(ProductModel productModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                Product product = Mapper.Map<Product>(productModel);
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = 1;
                if (productModel.ProductId != 0)
                    response.Status = await _unitOfWork.GetRepoInstance<Product>().Update(product);
                else
                {
                    product.CreatedDate = DateTime.Now;
                    product.CreatedBy = 1;
                    response.Status = await _unitOfWork.GetRepoInstance<Product>().Add(product);
                }
                response.Data = productModel;
                if (response.Status)
                    response.SuccessMessage = "Product added successfully";
                else
                    response.ErrorMessage = "Failed to add product";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> DeleteProduct(ProductModel productModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.Data = productModel;
                var _context = _unitOfWork.GetRepoInstance<Product>();
                Product product = await _context.GetById(productModel.ProductId);
                if (product == null)
                {
                    response.ErrorMessage = "Product not found";
                    return response;
                }

                _context.Delete(product);
                response.Status = true;
                response.SuccessMessage = "Product deleted successfully";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetAllProduct()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<Product> products = _unitOfWork.GetRepoInstance<Product>().GetAll().ToList();
                if (products.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<Product>>(products);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetProductById(long productId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                Product product = await _unitOfWork.GetRepoInstance<Product>().GetById(productId);
                if (product == null)
                {
                    response.ErrorMessage = "Product not found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<ProductModel>(product);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetProductsByCategoryId(long categoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<Product> products = _unitOfWork.GetRepoInstance<Product>().GetAll().Where(x => x.CategoryId == categoryId).ToList();
                if (products.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<Product>>(products);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetProductsBySubCategoryId(long subCategoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<Product> products = _unitOfWork.GetRepoInstance<Product>().GetAll().Where(x => x.SubCategoryId == subCategoryId).ToList();
                if (products.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<Product>>(products);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }
    }
}
