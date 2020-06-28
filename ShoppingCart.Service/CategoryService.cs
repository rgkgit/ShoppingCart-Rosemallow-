using AutoMapper;
using ShoppingCart.Model;
using ShoppingCart.Provider.EntityModel;
using ShoppingCart.Interface;
using ShoppingCart.Provider.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Service
{
    public class CategoryService : ICategory
    {
        UnitOfWork _unitOfWork;
        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> AddOrUpdateCategory(CategoryModel categoryModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                Category category = Mapper.Map<Category>(categoryModel);
                category.UpdatedDate = DateTime.Now;
                category.UpdatedBy = 1;
                if (category.CategoryId != 0)
                    response.Status = await _unitOfWork.GetRepoInstance<Category>().Update(category);
                else
                {
                    category.CreatedDate = DateTime.Now;
                    category.CreatedBy = 1;
                    response.Status = await _unitOfWork.GetRepoInstance<Category>().Add(category);
                }
                response.Data = categoryModel;
                if (response.Status)
                    response.SuccessMessage = "Category added successfully";
                else
                    response.ErrorMessage = "Failed to add category";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> DeleteCategory(CategoryModel categoryModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.Data = categoryModel;
                var _context = _unitOfWork.GetRepoInstance<Category>();
                Category category = await _context.GetById(categoryModel.CategoryId);
                if (category == null)
                {
                    response.ErrorMessage = "Category not found";
                    return response;
                }

                _context.Delete(category);
                response.Status = true;
                response.SuccessMessage = "Category deleted successfully";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetAllCategory()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<Category> categories = _unitOfWork.GetRepoInstance<Category>().GetAll().ToList();
                if (categories.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<Category>>(categories);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetCategoryById(long categoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                Category category = await _unitOfWork.GetRepoInstance<Category>().GetById(categoryId);
                if (category == null)
                {
                    response.ErrorMessage = "Category not found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<CategoryModel>(category);
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
