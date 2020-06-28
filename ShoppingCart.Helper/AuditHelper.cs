using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using ShoppingCart.Provider.EntityModel;
using ShoppingCart.Provider.Repository;

namespace ShoppingCart.Helper
{
    public class AuditHelperFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Set the value as true to log Response data which are required for further analysis
        /// In some cases the response data will be huge and hence need not be stored. In such cases set this to false
        /// By default this is false
        /// </summary>
        public bool IsResponseRequired { get; private set; }
        public AuditHelperFilter()
        {
            IsResponseRequired = false;
        }

        public AuditHelperFilter(bool isResponseRequired)
        {
            IsResponseRequired = isResponseRequired;
        }
        /// <summary>
        /// On Each activity audit the user, Api, request and response information. Response is tracked only in cases wherein
        /// its specified to be tracked
        /// </summary>
        /// <param name="actionExecutedContext">Filter context which contains all the action, controller, request and response data</param>
        /// <param name="cancellationToken">In case the action needs to be cancelled in between use this token for cancellation</param>
        /// <returns>This is an asyncronous silent process and need not return any result to the calling method</returns>
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    AuditInfo auditActivity = new AuditInfo();
                    //Fetch Employee Id from the context which is set in the Base Controller. Later this needs to be changed and fetched from decrypted token
                    //ClaimsIdentity identity = ((ClaimsIdentity)((ApiController)(actionExecutedContext.ActionContext.ControllerContext.Controller)).User.Identity);
                    //var hashedId = identity.Claims.Where(x => x.Type == "uuq").First().Value;
                    //auditActivity.EmployeeId = Convert.ToInt32(Decryptor.Decrypt(hashedId));
                    auditActivity.ControllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
                    auditActivity.ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                    auditActivity.RequestURL = actionExecutedContext.Request.RequestUri.AbsoluteUri;
                    //All the data thats posted to the API call is serialized into JSON format
                    auditActivity.RequestData = JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments);
                    string responseData = string.Empty;
                    //If Response needs to be stored then retreive the value and serialize into JSON format
                    if (IsResponseRequired)
                    {
                        //var contentData = (ObjectContent)actionExecutedContext.Response.Content;
                        //if (contentData != null)
                        //{
                        //    var typeOfResponse = contentData.ObjectType; //type of the returned object
                        //    var responseValue = contentData.Value; //holding the returned value
                        //    auditActivity.ResponseData = JsonConvert.SerializeObject(responseValue);
                        //}
                    }
                    //Save the audit details in the background
                    SaveAuditDetails(auditActivity);
                }
                catch (Exception ex)
                {
                    //ShoppingCartExceptionHandler.HandleError(ex);
                }
            });
        }

        public async void SaveAuditDetails(AuditInfo auditInfo)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                var repo = unitOfWork.GetRepoInstance<AuditDetail>();
                bool isSaved = false;
                AuditDetail auditDetail = new AuditDetail();
                auditDetail.ActionName = auditInfo.ActionName;
                auditDetail.ControllerName = auditInfo.ControllerName;
                auditDetail.CreatedDate = DateTime.UtcNow;
                auditDetail.Parameter = auditInfo.RequestData;
                auditDetail.RequestURL = auditInfo.RequestURL;
                auditDetail.ReturnValue = auditInfo.ResponseData;
                auditDetail.UserId = auditInfo.UserId;
                isSaved = await repo.Add(auditDetail);
            }
            catch (Exception ex)
            {
                string parameter = "Action Name : " + auditInfo.ActionName;
                parameter += ", Controller Name : " + auditInfo.ControllerName;
                parameter += ", Request Data : " + auditInfo.RequestData;
                parameter += ", Request URL : " + auditInfo.RequestURL;
                parameter += ", UserId : " + auditInfo.UserId;
                //ShoppingCartExceptionHandler.HandleError(ex, parameter);
            }
        }

    }
}
