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
    public class OrderService : IOrder
    {
        UnitOfWork _unitOfWork;
        public OrderService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> AddOrUpdateOrder(OrderModel orderModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                OrderDetail order = Mapper.Map<OrderDetail>(orderModel);
                Product product = _unitOfWork.GetRepoInstance<Product>().GetAll().Where(x => x.ProductId == orderModel.ProductId).SingleOrDefault();
                order.TotalAmount = orderModel.ItemCount * product.Price;
                order.PaymentStatus = "Pending";
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = 1;
                if (orderModel.OrderId != 0)
                    response.Status = await _unitOfWork.GetRepoInstance<OrderDetail>().Update(order);
                else
                {
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = 1;
                    response.Status = await _unitOfWork.GetRepoInstance<OrderDetail>().Add(order);
                }
                response.Data = orderModel;
                if (response.Status)
                    response.SuccessMessage = "Order added in cart successfully";
                else
                    response.ErrorMessage = "Failed to add order in cart";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> DeleteOrder(OrderModel orderModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.Data = orderModel;
                var _context = _unitOfWork.GetRepoInstance<OrderDetail>();
                OrderDetail order = await _context.GetById(orderModel.OrderId);
                if (order == null)
                {
                    response.ErrorMessage = "Order not found";
                    return response;
                }

                _context.Delete(order);
                response.Status = true;
                response.SuccessMessage = "Order deleted from cart successfully";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetAllOrders(long userId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<OrderDetail> orders = _unitOfWork.GetRepoInstance<OrderDetail>().GetAll().Where(x => x.UserId == userId).ToList();
                if (orders.Count() == 0)
                {
                    response.ErrorMessage = "No records found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<List<OrderDetail>>(orders);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> GetOrderById(long orderId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                OrderDetail order = await _unitOfWork.GetRepoInstance<OrderDetail>().GetById(orderId);
                if (order == null)
                {
                    response.ErrorMessage = "Order not found";
                    return response;
                }
                response.Status = true;
                response.Data = Mapper.Map<OrderModel>(order);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }

        public async Task<ResponseModel> PlaceOrder(long orderId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                OrderDetail order = _unitOfWork.GetRepoInstance<OrderDetail>().GetAll().Where(x => x.OrderId == orderId).SingleOrDefault();
                order.PaymentStatus = "Completed";
                order.IsOrderPlaced = true;
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = 1;
                response.Status = await _unitOfWork.GetRepoInstance<OrderDetail>().Update(order);

                response.Data = Mapper.Map<OrderModel>(order);
                if (response.Status)
                    response.SuccessMessage = "Order placed successfully";
                else
                    response.ErrorMessage = "Failed to place the order";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorMessage = "Internal server error. Please contact administrator.";
            }
            return response;
        }
    }
}
