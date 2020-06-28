using ShoppingCart.Provider.Repository;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using static ShoppingCart.Helper.Enums;

namespace ShoppingCart.Helper.Exceptions
{
    public class ShoppingCartExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private UnitOfWork _unitOfWork;

        public ShoppingCartExceptionFilterAttribute()
        {
            _unitOfWork = new UnitOfWork();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            ShoppingCartException ex;

            if (context.Exception.GetType() != typeof(ShoppingCartException))
            {
                ex = ShoppingCartExceptionHandler.HandleError(context.Exception);
            }
            else
            {
                ex = context.Exception as ShoppingCartException;
            }

            if (ex.IsCustomException)
            {
                throw new HttpResponseException(context.Request.CreateErrorResponse(ex.StatusCode, ex.ErrorMessage));
            }
            else
            {
                throw new HttpResponseException(context.Request.CreateErrorResponse((HttpStatusCode)ErrorNotificationType.VALIDATIONFAILURE, "Internal Server Error."));
            }

        }
    }
}
