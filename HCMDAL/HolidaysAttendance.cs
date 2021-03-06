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
    
    public partial class HolidaysAttendance
    {
        public HolidaysAttendance()
        {
            this.HolidaysAttendanceDetails = new HashSet<HolidaysAttendanceDetails>();
        }
    
        public int HolidayAttendanceID { get; set; }
        public int HolidaySettingID { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> OrganizationID { get; set; }
    
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual HolidaysSettings HolidaysSettings { get; set; }
        public virtual OrganizationsStructures OrganizationsStructures { get; set; }
        public virtual ICollection<HolidaysAttendanceDetails> HolidaysAttendanceDetails { get; set; }
    }
}
