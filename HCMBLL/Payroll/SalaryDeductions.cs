using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakafulDTO;

namespace HCMBLL
{
    public class SalaryDeductions
    {
        public List<Deduction> TakafulDeductions { get; set; }

        public List<GovernmentFundsBLL> GovernmentFundsDeductions { get; set; }

        public double RetirmentDeduction { get; set; }

        public double TotalDeductions { get; set; }
    }
}
