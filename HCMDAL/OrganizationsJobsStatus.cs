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
    
    public partial class OrganizationsJobsStatus
    {
        public OrganizationsJobsStatus()
        {
            this.OrganizationsJobs = new HashSet<OrganizationsJobs>();
        }
    
        public int OrganizationJobStatusID { get; set; }
        public string OrganizationJobStatusName { get; set; }
    
        public virtual ICollection<OrganizationsJobs> OrganizationsJobs { get; set; }
    }
}