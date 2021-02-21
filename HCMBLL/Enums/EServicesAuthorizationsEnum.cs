using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.Enums
{
    public enum EServicesAuthorizationsEnum
    {
        Done = 1,
        RejectedBecauseLoginOrganizationIsSameAsAuthorization = 2,
        RejectedBecauseThereIsNoManagerForThisOrganization = 3,
        RejectedBecauseOrganizationManagerIsNotAuthorized = 4,
    }
}
