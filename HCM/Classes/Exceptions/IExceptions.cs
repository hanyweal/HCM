using System;

namespace HCM.Classes
{
    public interface IEx
    {
        void ErrorLog(Exception ex);

        string Msg(Exception ex);
    }
}