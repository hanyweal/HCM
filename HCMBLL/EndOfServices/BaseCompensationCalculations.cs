using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public abstract class BaseCompensationCalculations
    {
        internal static double RetirementAge
        {
            get
            {
                return 60;
            }
        }

        internal static double AdditionalCompensationInSomeCases
        {
            get
            {
                return 1000000;
            }
        }
    }
}
