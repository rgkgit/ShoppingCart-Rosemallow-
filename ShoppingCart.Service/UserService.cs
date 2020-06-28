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
    public class UserService : IUser
    {
        UnitOfWork _unitOfWork;
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> AddOrUpdateUser(UserModel userModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                UserDetail user = Mapper.Map<UserDetail>(userModel);
                user.UpdatedDate = DateTime.Now;
                user.UpdatedBy = 1;
                if (user.UserId != 0)
                    response.Status = await _unitOfWork.GetRepoInstance<UserDetail>().Update(user);
                else
                {
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = 1;
                    response.Status = await _unitOfWork.GetRepoInstance<UserDetail>().Add(user);
                }
                response.Data = userModel;
                if (response.Status)
                    response.SuccessMessage = "User added successfully";
                else
                    response.ErrorMessage = "Failed to add user";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> DeleteUser(UserModel userModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.Data = userModel;
                var _context = _unitOfWork.GetRepoInstance<UserDetail>();
                UserDetail user = await _context.GetById(userModel.UserId);
                if (user == null)
                {
                    response.ErrorMessage = "User not found";
                    return response;
                }

                _context.Delete(user);
                response.Status = true;
                response.SuccessMessage = "User deleted successfully";
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetAllUser()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<UserDetail> users = _unitOfWork.GetRepoInstance<UserDetail>().GetAll().ToList();
                if (users.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<UserModel>>(users);
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetUserById(long userId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                UserDetail user = await _unitOfWork.GetRepoInstance<UserDetail>().GetById(userId);
                if (user == null)
                {
                    response.ErrorMessage = "User not found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<UserModel>(user);
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
