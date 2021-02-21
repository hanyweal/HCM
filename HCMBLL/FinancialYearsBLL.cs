using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public class FinancialYearsBLL : IEntity
    {
        public int FinancialYearID { get; set; }

        public int FinancialYear { get; set; }

        public DateTime FinancialYearStartDate { get; set; }

        public DateTime FinancialYearEndDate { get; set; }

        public FinancialYearsBLL GetByFinancialYear(int Year)
        {
            try
            {
                FinancialYears FinancialYear = new FinancialYearsDAL().GetByFinancialYear(Year);
                if (FinancialYear != null)
                {
                    return new FinancialYearsBLL()
                    {
                        FinancialYearID = FinancialYear.FinancialYearID,
                        FinancialYear = FinancialYear.FinancialYear,
                        FinancialYearStartDate = FinancialYear.StartDate,
                        FinancialYearEndDate = FinancialYear.EndDate,
                    };
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FinancialYearsBLL> GetFinancialYears()
        {
            try
            {
                List<FinancialYears> FinancialYears = new FinancialYearsDAL().GetFinancialYears();
                List<FinancialYearsBLL> FinancialYearsBLLList = null;
                if (FinancialYears != null)
                {
                    FinancialYearsBLLList = new List<FinancialYearsBLL>();
                    foreach (var item in FinancialYears)
                    {
                        FinancialYearsBLLList.Add(new FinancialYearsBLL()
                        {
                            FinancialYearID = item.FinancialYearID,
                            FinancialYear = item.FinancialYear,
                            FinancialYearStartDate = item.StartDate,
                            FinancialYearEndDate = item.EndDate,
                        });
                    }
                }
                return FinancialYearsBLLList;
            }
            catch
            {
                throw;
            }
        }
    }

}
