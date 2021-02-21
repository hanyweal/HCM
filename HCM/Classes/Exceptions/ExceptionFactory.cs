using ExceptionNameSpace;
using HCM.Classes.Exceptions;

namespace HCM.Classes
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