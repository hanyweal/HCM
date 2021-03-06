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
    
    public partial class Ranks
    {
        public Ranks()
        {
            this.RanksTicketsClasses = new HashSet<RanksTicketsClasses>();
            this.PassengerOrders = new HashSet<PassengerOrders>();
            this.BasicSalaries = new HashSet<BasicSalaries>();
            this.NextRanks = new HashSet<Ranks>();
            this.PromotionsRecords = new HashSet<PromotionsRecords>();
            this.OrganizationsJobs = new HashSet<OrganizationsJobs>();
        }
    
        public int RankID { get; set; }
        public int RankCategoryID { get; set; }
        public string RankName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<double> TransfareAllowance { get; set; }
        public Nullable<double> InternalDelegation { get; set; }
        public Nullable<double> ExternalDelegation { get; set; }
        public Nullable<bool> IsActiveForPromotion { get; set; }
        public Nullable<int> NextRankID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<int> Sorting { get; set; }
    
        public virtual RanksCategories RanksCategories { get; set; }
        public virtual ICollection<RanksTicketsClasses> RanksTicketsClasses { get; set; }
        public virtual ICollection<PassengerOrders> PassengerOrders { get; set; }
        public virtual ICollection<BasicSalaries> BasicSalaries { get; set; }
        public virtual ICollection<Ranks> NextRanks { get; set; }
        public virtual ICollection<PromotionsRecords> PromotionsRecords { get; set; }
        public virtual ICollection<OrganizationsJobs> OrganizationsJobs { get; set; }
        public virtual EmployeesCodes CreatedByNav { get; set; }
        public virtual EmployeesCodes LastUpdatedByNav { get; set; }
    }
}
