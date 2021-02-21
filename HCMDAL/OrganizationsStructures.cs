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
    
    public partial class OrganizationsStructures
    {
        public OrganizationsStructures()
        {
            this.OrganizationsStructures1 = new HashSet<OrganizationsStructures>();
            this.HolidaysAttendance = new HashSet<HolidaysAttendance>();
            this.Assignings = new HashSet<Assignings>();
            this.OrganizationsJobs = new HashSet<OrganizationsJobs>();
            this.EServicesAuthorizations = new HashSet<EServicesAuthorizations>();
            this.EVacationsRequests = new HashSet<EVacationsRequests>();
            this.OrganizationsManagers = new HashSet<OrganizationsManagers>();
        }
    
        public int OrganizationID { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> ManagerCodeID { get; set; }
        public Nullable<int> OrganizationParentID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<bool> ShowInECM { get; set; }
        public Nullable<bool> WorkingWithDigitalDecisionInECM { get; set; }
        public Nullable<int> AuthorizedToApproveOnEVacationsRequests { get; set; }
    
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual Branches Branches { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual ICollection<OrganizationsStructures> OrganizationsStructures1 { get; set; }
        public virtual OrganizationsStructures ParentOrganization { get; set; }
        public virtual ICollection<HolidaysAttendance> HolidaysAttendance { get; set; }
        public virtual ICollection<Assignings> Assignings { get; set; }
        public virtual ICollection<OrganizationsJobs> OrganizationsJobs { get; set; }
        public virtual EmployeesCodes EmployeesCodes1 { get; set; }
        public virtual EmployeesCodes EmployeesCodes11 { get; set; }
        public virtual ICollection<EServicesAuthorizations> EServicesAuthorizations { get; set; }
        public virtual ICollection<EVacationsRequests> EVacationsRequests { get; set; }
        public virtual ICollection<OrganizationsManagers> OrganizationsManagers { get; set; }
    }
}
