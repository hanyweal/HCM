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
    
    public partial class Assignings
    {
        public int AssigningID { get; set; }
        public Nullable<int> EmployeeCareerHistoryID { get; set; }
        public int AssigningTypID { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<int> ManagerCodeID { get; set; }
        public System.DateTime AssigningStartDate { get; set; }
        public Nullable<System.DateTime> AssigningEndDate { get; set; }
        public string ExternalCity { get; set; }
        public string ExternalOrganization { get; set; }
        public bool IsFinished { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<int> AssigningReasonID { get; set; }
        public Nullable<int> ExternalKSACityID { get; set; }
        public string AssigningReasonOther { get; set; }
        public Nullable<int> EndAssigningReasonID { get; set; }
        public string Notes { get; set; }
    
        public virtual AssigningsReasons AssigningsReasons { get; set; }
        public virtual AssigningsTypes AssigningsTypes { get; set; }
        public virtual EmployeesCareersHistory EmployeesCareersHistory { get; set; }
        public virtual EmployeesCodes ManagerCode { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual Jobs Jobs { get; set; }
        public virtual KSACities KSACities { get; set; }
        public virtual OrganizationsStructures OrganizationsStructures { get; set; }
        public virtual AssigningsReasons EndAssigningsReasonsNav { get; set; }
    }
}