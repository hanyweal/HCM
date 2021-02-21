using System;

namespace HCMBLL
{
    public class VacationBalanceTable
    {
        public int EmployeeCodeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ConsumedBalance { get; set; }
        public float RemainingBalance { get; set; }
    }
}
