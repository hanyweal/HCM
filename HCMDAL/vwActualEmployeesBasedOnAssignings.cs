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
    
    public partial class vwActualEmployeesBasedOnAssignings
    {
        public string EmployeeCodeNo { get; set; }
        public string EmployeeIDNo { get; set; }
        public string EmployeeNameAr { get; set; }
        public string RankName { get; set; }
        public string JobName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int RankCategoryID { get; set; }
        public string RankCategoryName { get; set; }
        public int RankID { get; set; }
        public string Expr1 { get; set; }
        public System.DateTime AssigningStartDate { get; set; }
        public Nullable<System.DateTime> AssigningEndDate { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<int> EmployeeCareerHistoryID { get; set; }
        public int EmployeeCodeID { get; set; }
        public Nullable<int> Sorting { get; set; }
    }
}