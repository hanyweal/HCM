//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HCMDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeesVacationsOpeningBalances
    {
        public int EmployeeVacationOpeningBalanceID { get; set; }
        public int EmployeeCodeID { get; set; }
        public int VacationTypeID { get; set; }
        public int OpeningBalance { get; set; }
    
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual VacationsTypes VacationsTypes { get; set; }
    }
}
