using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class OverTimesDaysBLL : CommonEntity, IEntity
    {
        public int OverTimeDayID { get; set; }

        public DateTime OverTimeDay { get; set; }

        public OverTimesBLL OverTimes { get; set; }

    }
}