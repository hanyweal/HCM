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
    
    public partial class ChangeLogs
    {
        public ChangeLogs()
        {
            this.ChangeDetailsLogs = new HashSet<ChangeDetailsLogs>();
        }
    
        public int ChangeLogID { get; set; }
        public string EntityName { get; set; }
        public Nullable<System.DateTime> DateChange { get; set; }
        public Nullable<int> EventTypeID { get; set; }
        public Nullable<int> EmployeeCodeID { get; set; }
    
        public virtual ICollection<ChangeDetailsLogs> ChangeDetailsLogs { get; set; }
        public virtual EventTypes EventTypes { get; set; }
    }
}