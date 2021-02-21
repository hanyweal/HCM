using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class PayrollsBLL
    { 
        public List<DateTime> GetDaysBetweenStartAndEndDate(int OverTimeID, int OverTimeDetailID)
        {
            List<DateTime> VacationDates = new List<DateTime>();
            OverTimesBLL Overtime = new OverTimesBLL().GetByOverTimeID(OverTimeID);
            OverTimesDetailsBLL OvertimeDetail = new OverTimesDetailsBLL().GetOverTimeDetailsByID(OverTimeDetailID);

            BaseVacationsBLL Vacation;
            List<BaseVacationsBLL> vacations = new BaseVacationsBLL().GetByEmployeeCodeID(OvertimeDetail.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);

            foreach (OverTimesDaysBLL OTDay in Overtime.OverTimesDays)
            {
                Vacation = vacations.FirstOrDefault(e => OTDay.OverTimeDay >= e.VacationStartDate && OTDay.OverTimeDay <= e.VacationEndDate);
                if (Vacation != null && Vacation.VacationID > 0)
                    VacationDates.Add(OTDay.OverTimeDay);
            }

            return VacationDates;
        }

        public List<OverTimesDetailsBLL> GetOverTimeAndVacationDaysByOverTimeID(int OvertimeID)
        {
            List<OverTimesDetailsBLL> lst = new List<OverTimesDetailsBLL>();            
            foreach (OverTimesDetailsBLL OvertimeDetail in new OverTimesDetailsBLL().GetOverTimeDetailsByOverTimeID(OvertimeID))
            { 
                OvertimeDetail.VacationDays = GetDaysBetweenStartAndEndDate(OvertimeID, OvertimeDetail.OverTimeDetailID);
                lst.Add(OvertimeDetail);
            }

            return lst;
        }
    }
}
