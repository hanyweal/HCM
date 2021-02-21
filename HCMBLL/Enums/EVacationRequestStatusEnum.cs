using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.Enums
{
    public enum EVacationRequestStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Refused = 3,
        CancelledByCreator = 4,
        CancelledByHR = 5,
        CancelledBySystem = 6
    }
}
