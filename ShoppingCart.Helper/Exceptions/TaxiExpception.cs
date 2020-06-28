using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static TaxiHelper.Enums;

namespace TaxiHelper.Exceptions
{
    public class TaxiException : Exception
    {
        public TaxiException()
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
            this.Severity = ErrorSeverity.MINOR;
        }

        public TaxiException(string message, Exception e, string parameters = "", HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, e)
        {
            this.ErrorMessage = message;
            this.StatusCode = statusCode;
            this.Severity = ErrorSeverity.MINOR;
            this.IsCustomException = true;
            this.Parameters = parameters;
        }

        public TaxiException(string message, string parameters = "", ErrorSeverity severity = ErrorSeverity.MINOR, bool isHandled = true, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            this.ErrorMessage = message;
            this.IsCustomException = true;
            this.StatusCode = statusCode;
            this.Severity = severity;
            this.IsHandled = IsHandled;
            this.Parameters = parameters;
        }

        public bool IsCustomException { get; set; }
        public bool IsHandled { get; set; }
        public ErrorSeverity Severity { get; set; }
        public string Location { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        public string TaxiInnerException { get; set; }
        public Type TaxiGetType { get; set; }
        public string TaxiStackTrace { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Parameters { get; set; }
        public Dictionary<object, object> Info { get; set; }
    }
}
