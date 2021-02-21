using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.Enums
{
    public enum ExceptionMessagesEnum
    {
        DataIsNotRight = 1,
        NoConnectionWithDBServer = 2,
        RecordAlreadyExist = 3,
        ErrorInStoredProcedure = 4,
        RecordRalatedWithOtherData = 5,
        UnhandledException = 6
    }
}
