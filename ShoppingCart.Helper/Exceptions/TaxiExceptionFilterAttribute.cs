using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using TaxiProvider.Repository;
using static TaxiHelper.Enums;

namespace TaxiHelper.Exceptions
{
    public class TaxiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private UnitOfWork _unitOfWork;

        public TaxiExceptionFilterAttribute()
        {
            _unitOfWork = new UnitOfWork();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            TaxiException ex;

            if (context.Exception.GetType() != typeof(TaxiException))
            {
                ex = TaxiExceptionHandler.HandleError(context.Exception);
            }
            else
            {
                ex = context.Exception as TaxiException;
            }

            if (ex.IsCustomException)
            {
                throw new HttpResponseException(context.Request.CreateErrorResponse(ex.StatusCode, ex.ErrorMessage));
            }
            else
            {
                throw new HttpResponseException(context.Request.CreateErrorResponse((HttpStatusCode)ErrorNotificationType.VALIDATIONFAILURE, "Internal Server Error"));
            }

        }
    }
}
