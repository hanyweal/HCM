using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class BasicSalariesDAL
    {
        public List<BasicSalaries> GetBasicSalaries()
        {
            try
            {
                var db = new HCMEntities();
                return db.BasicSalaries.Include("CareersDegrees").Include("Ranks").ToList();
            }
            catch
            {
                throw;
            }
        }

        public BasicSalaries GetBasicSalary(int RankID, int CareerDegreeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.BasicSalaries
                            .Include("CareersDegrees").Include("Ranks")
                            .FirstOrDefault(x => x.CareerDegreeID == CareerDegreeID & x.RankID == RankID);
            }
            catch
            {
                throw;
            }
        }
    }
}
