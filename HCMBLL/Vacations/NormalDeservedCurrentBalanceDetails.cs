using System;

namespace HCMBLL
{
    public class NormalDeservedCurrentBalanceDetails
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int MaturityYearBalance { get; set; }

        public int UnDeservedBalance { get; set; }

        public int DeservedBalance { get; set; }
    }
}