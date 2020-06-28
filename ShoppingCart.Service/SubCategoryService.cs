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
    public class SubCategoryService : ISubCategory
    {
        UnitOfWork _unitOfWork;
        public SubCategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> AddOrUpdateSubCategory(SubCategoryModel subCategoryModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                SubCategory category = Mapper.Map<SubCategory>(subCategoryModel);
                category.UpdatedDate = DateTime.Now;
                category.UpdatedBy = 1;
                if (subCategoryModel.SubCategoryId != 0)
                    response.Status = await _unitOfWork.GetRepoInstance<SubCategory>().Update(category);
                else
                {
                    category.CreatedDate = DateTime.Now;
                    category.CreatedBy = 1;
                    response.Status = await _unitOfWork.GetRepoInstance<SubCategory>().Add(category);
                }
                response.Data = subCategoryModel;
                if (response.Status)
                    response.SuccessMessage = "SubCategory added successfully";
                else
                    response.ErrorMessage = "Failed to add SubCategory";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> DeleteSubCategory(SubCategoryModel subCategoryModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.Data = subCategoryModel;
                var _context = _unitOfWork.GetRepoInstance<SubCategory>();
                SubCategory category = await _context.GetById(subCategoryModel.SubCategoryId);
                if (category == null)
                {
                    response.ErrorMessage = "SubCategory not found";
                    return response;
                }

                _context.Delete(category);
                response.Status = true;
                response.SuccessMessage = "SubCategory deleted successfully";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetAllSubCategory(long categoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<SubCategory> categories = _unitOfWork.GetRepoInstance<SubCategory>().GetAll().Where(x => x.CategoryId == categoryId).ToList();
                if (categories.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<SubCategory>>(categories);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetSubCategoryById(long subCategoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                SubCategory category = await _unitOfWork.GetRepoInstance<SubCategory>().GetById(subCategoryId);
                if (category == null)
                {
                    response.ErrorMessage = "SubCategory not found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<SubCategoryModel>(category);
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
