using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCMServices.Classes.Exceptions
{
    public interface IEx
    {
        void ErrorLog(Exception ex);

        string Msg(Exception ex);
    }
}