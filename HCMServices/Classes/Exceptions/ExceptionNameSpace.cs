using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI;
using Sahel.Api.Classes;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using HCMBLL.Enums;

namespace HCMServices.Classes.Exceptions
{
    public class EventViewerException : IEx
    {
        public void ErrorLog(Exception ex)
        {

        }

        public string Msg(Exception ex)
        {
            string ss = string.Empty;
            if (ex is SqlException)
            {
                SqlException sqlex = (SqlException)ex;
                ErrorLog(sqlex);
                ss = Message.Msg(sqlex);
            }
            else
            {
                ErrorLog(ex);
                ss = Message.Msg(ex);
            }
            return ss;
        }
    }

    public class FileException : IEx
    {
        //ErrorLog error_log;

        public void ErrorLog(Exception ex)
        {
            throw new NotImplementedException();
            //string PageName = HttpContext.Current.Request.Url.AbsolutePath;
            //error_log = new ErrorLog();
            //error_log.PageName = PageName;
            //error_log.EventName = ex.TargetSite.Name;
            //if (HttpContext.Current.Session["User_Name"] != null)
            //    error_log.UserName = HttpContext.Current.Session["User_Name"].ToString();
            //else
            //    error_log.UserName = string.Empty;

            //if (ex is SqlException)
            //{
            //    SqlException sqlex = (SqlException)ex;

            //    error_log.ErrorMessage = sqlex.Message;
            //    error_log.ErrorProcedure = sqlex.Procedure;
            //    error_log.ErrorNumber = sqlex.Number;
            //    error_log.ErrorLine = sqlex.LineNumber;
            //    error_log.ErrorState = sqlex.State;
            //    error_log.ErrorSeverity = sqlex.Class;
            //}
            //else if (ex is IOException)
            //{
            //    IOException ioex = (IOException)ex;
            //    error_log.ErrorMessage = ioex.Message;
            //    error_log.ErrorProcedure = "";
            //    error_log.ErrorNumber = -1;
            //    error_log.ErrorLine = -1;
            //    error_log.ErrorState = -1;
            //    error_log.ErrorSeverity = -1;
            //}
            //else
            //{
            //    error_log.ErrorMessage = ex.Message;
            //    error_log.ErrorProcedure = "";
            //    error_log.ErrorNumber = -1;
            //    error_log.ErrorLine = -1;
            //    error_log.ErrorState = -1;
            //    error_log.ErrorSeverity = -1;
            //}

            //using (StreamWriter sw = new StreamWriter(@"D:\\LogErrors\\SCM\\" + DateTime.Now.ToShortDateString() + " SCMLogError.txt", true))
            //{
            //    sw.WriteLine(DateTime.Now.ToString() + "    " + error_log.ErrorMessage + "    " + error_log.ErrorProcedure + "    " + error_log.EventName + "    " + error_log.ErrorNumber + "    " + error_log.ErrorState + "    " + error_log.ErrorSeverity + "    " + error_log.PageName + "    " + error_log.UserName);
            //    sw.Close();
            //}
        }

        public string Msg(Exception ex)
        {
            string ss = string.Empty;
            if (ex is SqlException)
            {
                SqlException sqlex = (SqlException)ex;
                ErrorLog(sqlex);
                ss = Message.Msg(sqlex);
            }
            else
            {
                ErrorLog(ex);
                ss = Message.Msg(ex);
            }
            return ss;
        }
    }

    public class DatabaseException : IEx
    {
        //  ErrorLogBLL error_logBLL;

        public void ErrorLog(Exception ex)
        {
            //error_logBLL = new ErrorLogBLL();

            //string PageTitle = HttpContext.Current.Request.Url.AbsolutePath;

            //error_logBLL.PageTitle = PageTitle;
            //error_logBLL.EventName = ex.TargetSite.Name;
            //if (HttpContext.Current.Session["User_Name"] != null)
            //    error_logBLL.UserName = HttpContext.Current.Session["User_Name"].ToString();
            //else
            //    error_logBLL.UserName = string.Empty;

            //if (ex is SqlException)
            //{
            //    SqlException sqlex = (SqlException)ex;
            //    error_logBLL.ErrorMessage = sqlex.Message;
            //    error_logBLL.ErrorProcedure = sqlex.Procedure;
            //    error_logBLL.ErrorNumber = sqlex.Number;
            //    error_logBLL.ErrorLine = sqlex.LineNumber;
            //    error_logBLL.ErrorState = sqlex.State;
            //    error_logBLL.ErrorSeverity = sqlex.Class;
            //}
            //else if (ex is IOException)
            //{
            //    IOException ioex = (IOException)ex;
            //    error_logBLL.ErrorMessage = ioex.Message;
            //    error_logBLL.ErrorProcedure = string.Empty;
            //    error_logBLL.ErrorNumber = -1;
            //    error_logBLL.ErrorLine = -1;
            //    error_logBLL.ErrorState = -1;
            //    error_logBLL.ErrorSeverity = -1;
            //}
            //else
            //{
            //    error_logBLL.ErrorMessage = ex.Message;
            //    error_logBLL.ErrorProcedure = string.Empty;
            //    error_logBLL.ErrorNumber = -1;
            //    error_logBLL.ErrorLine = -1;
            //    error_logBLL.ErrorState = -1;
            //    error_logBLL.ErrorSeverity = -1;
            //}

            //error_logBLL.AddErrorLog();
        }

        public string Msg(Exception ex)
        {
            string ss = string.Empty;
            if (ex is SqlException)
            {
                SqlException sqlex = (SqlException)ex;
                ErrorLog(sqlex);
                ss = Message.Msg(sqlex);
            }
            else
            {
                ErrorLog(ex);
                ss = Message.Msg(ex);
            }
            return ss;
        }
    }

    //public class MissingRequiredFieldsException : Exception
    //{
    //    private string _message;
    //    public MissingRequiredFieldsException(string message)
    //        : base(message)
    //    {
    //        _message = message;
    //    }
    //}

    //[Serializable]
    public class CustomException : Exception
    {
        private string _message;
        public CustomException(string message)
            : base(message)
        {
            _message = message;
        }
    }

    static class Message
    {
        public static string Msg(Exception ex)
        {
            string message = string.Empty;
            if (ex is CustomException)
            {
                message = ex.Message;
            }
            else if (ex is SqlException)
            {
                message = SqlExceptionMessages(ex);
            }
            else if (ex is DbUpdateException)
            {
                DbUpdateException DbUpdateEx = (DbUpdateException)ex;
                if (DbUpdateEx.InnerException.InnerException is SqlException)
                {
                    message = SqlExceptionMessages(DbUpdateEx.InnerException.InnerException);
                }
            }
            else if (ex is System.Reflection.TargetInvocationException)
            {
                DbUpdateException DbUpdateEx = (DbUpdateException)ex.InnerException;
                if (DbUpdateEx.InnerException.InnerException is SqlException)
                {
                    message = SqlExceptionMessages(DbUpdateEx.InnerException.InnerException);
                }
            }
            else if (ex is FormatException)
            {
                message = ExceptionMessagesEnum.DataIsNotRight.ToString();
            }
            else
                message = ExceptionMessagesEnum.UnhandledException.ToString();

            return message;
        }

        private static string SqlExceptionMessages(Exception ex)
        {
            SqlException sqlex = (SqlException)ex;
            switch (sqlex.Number)
            {
                case -1:
                case 53:
                    return ExceptionMessagesEnum.NoConnectionWithDBServer.ToString();
                case 2601:
                case 2627:
                    return ExceptionMessagesEnum.RecordAlreadyExist.ToString(); //Resources.Globalization.RecordAlreadyExistText; //
                case 201:
                    return ExceptionMessagesEnum.ErrorInStoredProcedure.ToString();
                case 547:
                    return ExceptionMessagesEnum.RecordRalatedWithOtherData.ToString(); //Resources.Globalization.RecordRalatedWithOtherDataText.ToString(); 
                default:
                    return "Error Number : " + sqlex.Number + "NewLine" + Globals.Utilities.CombineListOfStrings(sqlex.Message.ToString(), '\''); 
            }
        }
    }
}