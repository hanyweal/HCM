using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class FinancialYearsDAL
    {
        public FinancialYears GetByFinancialYear(int Year)
        {
            try
            {
                var db = new HCMEntities();
                return db.FinancialYears.SingleOrDefault(x => x.FinancialYear.Equals(Year));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FinancialYears> GetFinancialYears()
        {
            try
            {
                var db = new HCMEntities();
                return db.FinancialYears.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
