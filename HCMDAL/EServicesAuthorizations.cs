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
    
    public partial class EServicesAuthorizations
    {
        public int EServiceAuthorizationID { get; set; }
        public int OrganizationID { get; set; }
        public int EServiceTypeID { get; set; }
        public int AuthorizedPersonCodeID { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    
        public virtual EmployeesCodes AuthorizedPersonNav { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual EServicesTypes EServicesTypes { get; set; }
        public virtual OrganizationsStructures OrganizationsStructures { get; set; }
    }
}
