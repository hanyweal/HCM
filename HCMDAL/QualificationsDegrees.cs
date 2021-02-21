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
    
    public partial class QualificationsDegrees
    {
        public QualificationsDegrees()
        {
            this.EmployeesQualifications = new HashSet<EmployeesQualifications>();
            this.JobsCategories = new HashSet<JobsCategories>();
            this.JobsCategoriesQualifications = new HashSet<JobsCategoriesQualifications>();
            this.PromotionsRecordsQualificationsPoints = new HashSet<PromotionsRecordsQualificationsPoints>();
            this.Qualifications = new HashSet<Qualifications>();
            this.PromotionsRecordsEmployees = new HashSet<PromotionsRecordsEmployees>();
            this.PromotionsRecordsEmployeesEducationsDetails = new HashSet<PromotionsRecordsEmployeesEducationsDetails>();
        }
    
        public int QualificationDegreeID { get; set; }
        public string QualificationDegreeName { get; set; }
        public Nullable<decimal> DirectPoints { get; set; }
        public Nullable<decimal> IndirectPoints { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<int> Weight { get; set; }
    
        public virtual EmployeesCodes EmployeesCodes { get; set; }
        public virtual ICollection<EmployeesQualifications> EmployeesQualifications { get; set; }
        public virtual ICollection<JobsCategories> JobsCategories { get; set; }
        public virtual ICollection<JobsCategoriesQualifications> JobsCategoriesQualifications { get; set; }
        public virtual ICollection<PromotionsRecordsQualificationsPoints> PromotionsRecordsQualificationsPoints { get; set; }
        public virtual ICollection<Qualifications> Qualifications { get; set; }
        public virtual ICollection<PromotionsRecordsEmployees> PromotionsRecordsEmployees { get; set; }
        public virtual ICollection<PromotionsRecordsEmployeesEducationsDetails> PromotionsRecordsEmployeesEducationsDetails { get; set; }
    }
}
