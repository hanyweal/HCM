using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class SalaryBenefits
    {
        public double BasicSalary { get; set; }

        public double TransfareAllowance { get; set; }

        public double TotalAllowances { get; set; }

        public List<EmployeesAllowancesBLL> EmployeesAllowances { get; set; }
    }
}
