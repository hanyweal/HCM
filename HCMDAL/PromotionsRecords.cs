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
    
    public partial class PromotionsRecords
    {
        public PromotionsRecords()
        {
            this.PromotionsRecordsJobsVacancies = new HashSet<PromotionsRecordsJobsVacancies>();
            this.PromotionsRecordsQualificationsPoints = new HashSet<PromotionsRecordsQualificationsPoints>();
            this.PromotionsRecordsEmployees = new HashSet<PromotionsRecordsEmployees>();
        }
    
        public int PromotionRecordID { get; set; }
        public int PromotionRecordNo { get; set; }
        public System.DateTime PromotionRecordDate { get; set; }
        public int RankID { get; set; }
        public int JobCategoryID { get; set; }
        public int PromotionPeriodID { get; set; }
        public int PromotionRecordStatusID { get; set; }
        public System.DateTime OpeningTime { get; set; }
        public int OpeningBy { get; set; }
        public Nullable<System.DateTime> InstallationTime { get; set; }
        public Nullable<int> InstallationBy { get; set; }
        public Nullable<System.DateTime> ClosingTime { get; set; }
        public Nullable<int> ClosingBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> PreferenceTime { get; set; }
        public Nullable<int> PreferenceBy { get; set; }
        public Nullable<System.DateTime> PromotionDate { get; set; }
    
        public virtual JobsCategories JobsCategories { get; set; }
        public virtual PromotionsRecordsStatus PromotionsRecordsStatus { get; set; }
        public virtual Ranks Ranks { get; set; }
        public virtual ICollection<PromotionsRecordsJobsVacancies> PromotionsRecordsJobsVacancies { get; set; }
        public virtual ICollection<PromotionsRecordsQualificationsPoints> PromotionsRecordsQualificationsPoints { get; set; }
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual EmployeesCodes EmployeesCodes1 { get; set; }
        public virtual EmployeesCodes EmployeesCodes2 { get; set; }
        public virtual EmployeesCodes EmployeesCodes3 { get; set; }
        public virtual EmployeesCodes EmployeesCodes4 { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual ICollection<PromotionsRecordsEmployees> PromotionsRecordsEmployees { get; set; }
        public virtual PromotionsPeriods PromotionsPeriods { get; set; }
    }
}
