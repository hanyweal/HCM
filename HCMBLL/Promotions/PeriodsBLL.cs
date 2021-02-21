using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public class PeriodsBLL
    {
        public int PeriodID { get; set; }

        public string PeriodName { get; set; }

        public List<PeriodsBLL> GetPeriods()
        {
            try
            {
                List<Periods> PeriodsList = new PeriodsDAL().GetPeriods();
                List<PeriodsBLL> PeriodsBLLList = new List<PeriodsBLL>();
                foreach (var item in PeriodsList)
                {
                    PeriodsBLLList.Add(new PeriodsBLL() { PeriodID = item.PeriodID, PeriodName = item.PeriodName });
                }
                return PeriodsBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}