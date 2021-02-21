using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCMServices.Classes.Exceptions
{
    public class ExceptionFactory
    {
        public static IEx CreateException(ExceptionEnum i)
        {
            if (i == ExceptionEnum.DatabaseException)
                return new DatabaseException();
            else if (i == ExceptionEnum.FileException)
                return new FileException();
            else
                return new EventViewerException();
        }
    }
}