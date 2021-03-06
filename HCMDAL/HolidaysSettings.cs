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
    
    public partial class HolidaysSettings
    {
        public HolidaysSettings()
        {
            this.HolidaysAttendance = new HashSet<HolidaysAttendance>();
        }
    
        public int HolidaySettingID { get; set; }
        public int HolidayTypeID { get; set; }
        public int MaturityYearID { get; set; }
        public System.DateTime HolidaySettingStartDate { get; set; }
        public System.DateTime HolidaySettingEndDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
        public virtual HolidaysTypes HolidaysTypes { get; set; }
        public virtual MaturityYearsBalances MaturityYearsBalances { get; set; }
        public virtual ICollection<HolidaysAttendance> HolidaysAttendance { get; set; }
    }
}
