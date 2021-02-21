using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.Enums
{
    public enum EServicesProxiesEnum
    {
        Done = 1,
        RejectedBecauseLoginManagerIsSameAsProxyEmployee = 2,
        RejectedBecauseThereIsAlreadyActiveProxyExist = 3,
        RejectedBecauseStartDateRequried = 4,
        RejectedBecauseEServiceProxyIsNotActive = 5,
        RejectedBecauseThereIsPendingEVacationRequestExist = 6,
        RejectedBecauseThereIsVacationExist = 7,
    }
}
