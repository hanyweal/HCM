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
    
    public partial class JobsGeneralGroups
    {
        public JobsGeneralGroups()
        {
            this.JobsGroups = new HashSet<JobsGroups>();
        }
    
        public int JobGeneralGroupID { get; set; }
        public string JobGeneralGroupName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
    
        public virtual ICollection<JobsGroups> JobsGroups { get; set; }
    }
}