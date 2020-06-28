using ShoppingCart.Helper.Exceptions;
using ShoppingCart.Provider;
using ShoppingCart.Provider.EntityModel;
using ShoppingCartHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using static ShoppingCart.Helper.Enums;

namespace ShoppingCart
{
    public class ShoppingCartExceptionHandler
    {
        /// <summary>
        /// Handle and log the exception into a file and datbase.
        /// </summary>
        /// <param name="ex">System.Exception</param>
        /// <param name="location">Location where exception has occurred, if not specified it will pick from CallerMemberName attribute</param>
        /// <param name="severity">CRITICAL, MAJOR, MODERATE, MINOR-Default, COSMETIC</param>
        /// <param name="code">You can specify a error code, if not this will be empty</param>
        /// <returns></returns>
        public static ShoppingCartException HandleError(Exception ex, string parameters = "", [CallerMemberName] string location = "", ErrorSeverity severity = ErrorSeverity.MINOR, string code = "", [CallerFilePath] string filePath = "")
        {
            ShoppingCartException ShoppingCartEx = null;

            if (ex.GetType() == typeof(ShoppingCartException))
            {
                ShoppingCartEx = ex as ShoppingCartException;
            }
            else
            {
                ShoppingCartEx = new ShoppingCartException(ex.Message, ex);
            }

            if (severity != ErrorSeverity.COSMETIC)
            {
                ShoppingCartEx.Severity = severity;
            }

            ShoppingCartEx.Location = filePath + "-" + location;

            if (!string.IsNullOrEmpty(parameters))
            {
                ShoppingCartEx.Parameters = parameters;
            }

            ShoppingCartEx.Code = "HttpStatusCode: " + ((int)ShoppingCartEx.StatusCode).ToString();

            if (!string.IsNullOrEmpty(code))
            {
                ShoppingCartEx.Code = " ErrorCode: " + code;
            }

            if (!ShoppingCartEx.IsHandled)
            {
                ShoppingCartEx.Source = "API";
                ErrorLogDatabase(ShoppingCartEx);
                ErrorLogText(ShoppingCartEx);
                ShoppingCartEx.IsHandled = true;
            }

            return ShoppingCartEx;
        }

        public static void ErrorLogDatabase(ShoppingCartException error)
        {
            try
            {
                using (ShoppingCartDbContext context = new ShoppingCartDbContext())
                {
                    ErrorLog errorLog = new ErrorLog()
                    {
                        Severity = error.Severity.ToString(),
                        Location = error.Location,
                        Code = error.Code,
                        //Source = error.Source,
                        Message = error.Message
                    };
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        errorLog.Message += Environment.NewLine + "Custom Message: " + error.ErrorMessage;
                    }

                    if (error.InnerException != null)
                    {
                        errorLog.Message += Environment.NewLine + "Inner Exception: ";

                        if (error.Message != error.InnerException.Message)
                        {
                            errorLog.Message += Environment.NewLine + error.InnerException.Message;
                        }

                        if (error.InnerException.InnerException != null)
                        {
                            errorLog.Message += Environment.NewLine + error.InnerException.InnerException.Message;

                            if (error.InnerException.InnerException.InnerException != null)
                            {
                                errorLog.Message += Environment.NewLine + error.InnerException.InnerException.InnerException.Message;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(error.Parameters))
                    {
                        errorLog.Detail = "Parameters:" + Environment.NewLine + error.Parameters;
                    }

                    if (error.GetBaseException().GetType() == typeof(DbEntityValidationException))
                    {
                        errorLog.Detail += Environment.NewLine + "DBEntityValidationException:";

                        DbEntityValidationException dbex = (DbEntityValidationException)error.GetBaseException();
                        List<DbEntityValidationResult> errlist = dbex.EntityValidationErrors.ToList();
                        foreach (DbEntityValidationResult err in errlist)
                        {
                            foreach (DbValidationError er in err.ValidationErrors)
                            {
                                errorLog.Detail += Environment.NewLine + er.ErrorMessage;
                            }
                        }
                    }

                    if (HttpContext.Current != null)
                    {
                        try
                        {
                            if (UserContextHelper.Context != null)
                            {
                                errorLog.Detail += Environment.NewLine + "UserID:" + UserContextHelper.Context.UserID.ToString();
                                errorLog.UserID = UserContextHelper.Context.UserID;
                            }
                        }
                        catch (Exception)
                        {
                            //should not be handled as context could throw exception if a method is invoked byjob
                        }
                        errorLog.Detail += Environment.NewLine + "Input Detail:";
                        errorLog.Detail += HttpContext.Current.Request.RequestType + "-" + HttpContext.Current.Request.RawUrl;
                        try
                        {
                            if (HttpContext.Current.Request.InputStream != null)
                            {
                                string data = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                                if (!string.IsNullOrEmpty(data))
                                {
                                    errorLog.Detail += Environment.NewLine + "Data:";
                                    errorLog.Detail += data;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //In some cases the inputstream property is not readable so skipping that exception
                        }
                    }
                   

                    errorLog.Detail += Environment.NewLine + "Stack trace:";
                    errorLog.Detail += error.StackTrace ?? string.Empty;

                    if (error.InnerException != null)
                    {
                        errorLog.Detail += error.InnerException.StackTrace ?? string.Empty;
                    }

                    errorLog.LoggedOn = DateTime.UtcNow;
                    errorLog.ISDTime = DateTime.UtcNow.AddHours(5).AddMinutes(30);

                    if (errorLog != null)
                    {
                        context.ErrorLogs.Add(errorLog);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                error.ErrorMessage = "Log error in database failed - " + ex.Message + Environment.NewLine + error.ErrorMessage;
                ErrorLogText(error);
            }
        }

        public static void ErrorLogText(ShoppingCartException error)
        {
            StreamWriter streamWriter = null;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = string.Empty;
            string fileName = "Error_" + DateTime.UtcNow.ToString("dd_MMM_yyyy") + ".txt";
            try
            {
                if (!Directory.Exists(path + "Logs"))
                {
                    Directory.CreateDirectory(path + "Logs");
                }

                logFilePath = path + @"Logs\" + fileName;

                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    return;
                }

                streamWriter = new StreamWriter(logFilePath, true);
                streamWriter.WriteLine("UTC Time : {0}", DateTime.UtcNow.ToString("dd MMM yyyy HH:mm:ss"));
                streamWriter.WriteLine("ISD Time : {0}", DateTime.UtcNow.AddHours(5).AddMinutes(30).ToString("dd MMM yyyy HH:mm:ss"));
                streamWriter.WriteLine("Exception: {0}", error.GetBaseException() != null ? error.GetBaseException().GetType().ToString() : error.GetType().ToString());
                streamWriter.WriteLine("Message  : {0}", error.Message);
                streamWriter.WriteLine("Source  : {0}", error.Source);
                if (!string.IsNullOrEmpty(error.ErrorMessage))
                {
                    streamWriter.WriteLine("Custom Message: {0}", error.ErrorMessage);
                }
                if (!string.IsNullOrEmpty(error.Location))
                {
                    streamWriter.WriteLine("Location : {0}", error.Location);
                }
                streamWriter.WriteLine("Severity : {0}", error.Severity.ToString());
                if (!string.IsNullOrEmpty(error.Code))
                {
                    streamWriter.WriteLine("Code     : {0}", error.Code);
                }
                if (error.InnerException != null)
                {
                    streamWriter.WriteLine("Inner Exception:");
                    streamWriter.WriteLine("----------------");
                    if (error.Message != error.InnerException.Message)
                    {
                        streamWriter.WriteLine(error.InnerException.Message);
                    }
                    if (error.InnerException.InnerException != null)
                    {
                        streamWriter.WriteLine(error.InnerException.InnerException.Message);
                        if (error.InnerException.InnerException.InnerException != null)
                        {
                            streamWriter.WriteLine(error.InnerException.InnerException.InnerException.Message);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(error.Parameters))
                {
                    streamWriter.WriteLine(Environment.NewLine + "Parameters:");
                    streamWriter.WriteLine(error.Parameters);
                }

                if (error.GetBaseException().GetType() == typeof(DbEntityValidationException))
                {
                    streamWriter.WriteLine(Environment.NewLine + "DBEntityValidationException:");
                    streamWriter.WriteLine("--------------------------------------");
                    DbEntityValidationException dbex = (DbEntityValidationException)error.GetBaseException();
                    List<DbEntityValidationResult> errlist = dbex.EntityValidationErrors.ToList();
                    foreach (DbEntityValidationResult err in errlist)
                    {
                        foreach (DbValidationError er in err.ValidationErrors)
                        {
                            streamWriter.WriteLine(Environment.NewLine + er.ErrorMessage);
                        }
                    }
                }

                if (HttpContext.Current != null)
                {
                    try
                    {
                        if (UserContextHelper.Context != null)
                        {
                            streamWriter.WriteLine("UserID:" + UserContextHelper.Context.UserID.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        //should not be handled as context could throw exception if a method is invoked byjob
                    }
                    streamWriter.WriteLine("Input Detail:");
                    streamWriter.WriteLine("-----------");
                    streamWriter.WriteLine(HttpContext.Current.Request.RequestType + "-" + HttpContext.Current.Request.RawUrl);
                    try
                    {
                        if (HttpContext.Current.Request.InputStream != null)
                        {
                            string data = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                            if (!string.IsNullOrEmpty(data))
                            {
                                streamWriter.WriteLine("Data:");
                                streamWriter.WriteLine("-----------");
                                streamWriter.WriteLine(data);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //In some cases the inputstream property is not readable so skipping that exception
                    }

                }

                streamWriter.WriteLine("Stack Trace:");
                streamWriter.WriteLine("-----------");
                if (error.StackTrace != null)
                {
                    streamWriter.WriteLine(error.StackTrace);
                }

                if (error.InnerException != null)
                {
                    if (error.InnerException.StackTrace != null)
                    {
                        streamWriter.WriteLine(error.InnerException.StackTrace);
                    }
                }
                streamWriter.WriteLine("-----------------------------------------------------------------------------------------------------------");
            }
            catch (Exception)
            {

            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }
    }
}
