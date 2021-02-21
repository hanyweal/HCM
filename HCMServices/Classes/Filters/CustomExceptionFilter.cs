using HCMServices.Classes.Exceptions;
using PSADTO;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Filters;
using HCMBLL;

namespace HCMServices.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        ErrorsLogsBLL PSAServiceObj = new ErrorsLogsBLL();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            IEx iex = ExceptionFactory.CreateException(ExceptionEnum.DatabaseException);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(iex.Msg(actionExecutedContext.Exception))
            };

            //log error into PSA DB 
            PSAServiceObj.AddErrorLog(CreateErrorLogObject(actionExecutedContext));

            actionExecutedContext.Response = response;
        }

        private ErrorsLogs CreateErrorLogObject(HttpActionExecutedContext actionExecutedContext)
        {
            ErrorsLogs ErrorLog = new ErrorsLogs();

            ErrorLog.Project = new Projects() { ProjectID = Convert.ToInt32(ConfigurationManager.AppSettings["ProjectID"]) };
            ErrorLog.URL = actionExecutedContext.Request.RequestUri.AbsolutePath;
            ErrorLog.Method = actionExecutedContext.Request.Method.Method;
            ErrorLog.Message = GetErrorMessage(actionExecutedContext.Exception);
            ErrorLog.Source = actionExecutedContext.Exception.Source;
            ErrorLog.StackTrace = actionExecutedContext.Exception.StackTrace;
            ErrorLog.TargetSiteName = actionExecutedContext.Exception.TargetSite.Name;

            ErrorLog.CreatedBy = new Employees() { EmployeeCodeID = 22915 };                 ////????  later we'll change
            ErrorLog.CreatedOn = DateTime.Now;

            return ErrorLog;
        }

        private string GetErrorMessage(Exception exception)
        {
            return exception.Message + Environment.NewLine + (exception.InnerException != null ? GetErrorMessage(exception.InnerException) : "");
        }
    }
}