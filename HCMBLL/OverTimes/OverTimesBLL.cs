using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMBLL.DTO;
using HCMBLL.Enums;
using HCMDAL;

namespace HCMBLL
{
    public class OverTimesBLL : ExpensesBLL
    {
        public int OverTimeID { get; set; }

        public DateTime OverTimeStartDate { get; set; }

        public int OverTimePeriod
        {
            get
            {
                return this.OverTimeEndDate.Subtract(this.OverTimeStartDate).Days + 1;
            }
        }

        public DateTime OverTimeEndDate { get; set; }

        public double WeekWorkHoursAvg { get; set; }

        public double SaturdayHoursAvg { get; set; }

        public double FridayHoursAvg { get; set; }

        public int OverTimeFiscalYear
        {
            get
            {
                //int year = new Globals.Calendar().IsHijri(DelegationStartDate) ? Globals.Calendar.GetGregYear(DelegationStartDate) : DelegationStartDate.Year;
                //int year = Globals.Calendar.GetUmAlQuraYear(OverTimeStartDate.Date);
                //return new FinancialYearsBLL().GetByFinancialYear(year).FinancialYear;

                int year = OverTimeStartDate.Year;
                return year;
            }
        }

        public string Requester { get; set; }

        public string Tasks { get; set; }

        public List<OverTimesDetailsBLL> OverTimesDetails { get; set; }

        public List<OverTimesDaysBLL> OverTimesDays { get; set; }

        public Result Add()
        {
            Result result;
            // validate employees PassengerOrder limit
            if (this.OverTimesDetails == null || this.OverTimesDetails.Count <= 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(OverTimeValidationEnum);
                result.EnumMember = OverTimeValidationEnum.RejectedBecauseEmployeeRequired.ToString();

                return result;
            }

            OverTimes overTime = new OverTimes();
            overTime.CreatedDate = DateTime.Now;
            overTime.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            overTime.OverTimeEndDate = this.OverTimeEndDate;
            overTime.OverTimeStartDate = this.OverTimeStartDate;
            overTime.Tasks = this.Tasks;
            overTime.FridayHoursAvg = this.FridayHoursAvg;
            overTime.SaturdayHoursAvg = this.SaturdayHoursAvg;
            overTime.WeekWorkHoursAvg = this.WeekWorkHoursAvg;
            overTime.Requester = this.Requester;
            this.OverTimesDetails.ForEach(c => overTime.OverTimesDetails.Add(new OverTimesDetails()
            {
                CreatedBy = this.LoginIdentity.EmployeeCodeID,
                CreateDate = DateTime.Now,
                EmployeeCareerHistoryID = c.EmployeeCareerHistory.EmployeeCareerHistoryID
            }));
            this.OverTimesDays.ForEach(c => overTime.OverTimesDays.Add(new OverTimesDays()
            {
                OverTimeDay=c.OverTimeDay
            }));
            this.OverTimeID = new OverTimesDAL().Insert(overTime);
            result = new Result();
            result.Entity = this;
            result.EnumMember = OverTimeValidationEnum.Done.ToString();
            result.EnumType = typeof(OverTimeValidationEnum);
            return result;
        }

        public OverTimesBLL GetByOverTimeID(int OverTimeID)
        {
            OverTimesBLL OverTimesBLL = null;
            OverTimes OverTime = new OverTimesDAL().GetByOverTimeID(OverTimeID);
            if (OverTime != null)
            {
                OverTimesBLL = new OverTimesBLL().MapOverTime(OverTime);
            }
            return OverTimesBLL;
        }

        public List<OverTimesBLL> GetOverTimes()
        {
            List<OverTimesBLL> OverTimeBLLList = new List<OverTimesBLL>();
            List<OverTimes> OverTimes = new OverTimesDAL().GetOverTimes();
            foreach (var item in OverTimes)
            {
                OverTimeBLLList.Add(new OverTimesBLL().MapOverTime(item));
            }
            return OverTimeBLLList;
        }

        public List<OverTimesBLL> GetOverTimes(out int totalRecordsOut, out int recFilterOut)
        {
            List<OverTimesBLL> OverTimeBLLList = new List<OverTimesBLL>();
            List<OverTimes> OverTimes = new OverTimesDAL()
            {
                search = Search,
                order = Order,
                orderDir = OrderDir,
                startRec = StartRec,
                pageSize = PageSize
            }.GetOverTimes(out  totalRecordsOut, out recFilterOut);
            foreach (var item in OverTimes)
            {
                OverTimeBLLList.Add(new OverTimesBLL().MapOverTime(item));
            }
            return OverTimeBLLList;
        }

        public Result Remove()
        {
            try
            {
                Result result = null;
                new OverTimesDAL().Delete(new OverTimes() { OverTimeID = OverTimeID }, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(OverTimeValidationEnum),
                    EnumMember = OverTimeValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public Result Update()
        {
            Result result = null;
            List<OverTimesDetailsBLL> OverTimeDetailBLLList = new OverTimesDetailsBLL().GetOverTimeDetailsByOverTimeID(this.OverTimeID);

            // validate employees  
            if (OverTimeDetailBLLList == null || OverTimeDetailBLLList.Count <= 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(OverTimeValidationEnum);
                result.EnumMember = OverTimeValidationEnum.RejectedBecauseEmployeeRequired.ToString();

                return result;
            }
            else
            {
                OverTimes overTime = new OverTimes()
                {
                    OverTimeID = this.OverTimeID,
                    Tasks = this.Tasks,
                    WeekWorkHoursAvg = this.WeekWorkHoursAvg,
                    FridayHoursAvg = this.FridayHoursAvg,
                    SaturdayHoursAvg = this.SaturdayHoursAvg,
                    Requester = this.Requester,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                new OverTimesDAL().Update(overTime);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(OverTimeValidationEnum),
                    EnumMember = OverTimeValidationEnum.Done.ToString()
                };
            }

            return result;
        }

        public List<OverTimesBLL> GetByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            List<OverTimesBLL> OverTimesBll = new EmployeesCodesBLL().GetOverTimesByEmployeeCodeID(EmployeeCodeID);
            List<OverTimesBLL> OverTimesWithDaysBll = new List<OverTimesBLL>();
            foreach (var OverTime in OverTimesBll)
            {
                if (OverTime.OverTimesDays.Exists(c => c.OverTimeDay >= StartDate && c.OverTimeDay <= EndDate))
                {
                    OverTimesWithDaysBll.Add(OverTime);
                }
            }
            return OverTimesWithDaysBll;

        }

        public IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> GetEmployeesOverTimesBasedOnAssigningsAsRanksCategories(DateTime FromDate, DateTime ToDate, int OrganizationID)
        {
            try
            {
                List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);
                DateTime FromDateGr = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", FromDate.Day, FromDate.Month, FromDate.Year)), new CultureInfo("en-US"));
                DateTime ToDateGr = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", ToDate.Day, ToDate.Month, ToDate.Year)), new CultureInfo("en-US"));
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesHaveOverTimes = new List<vwActualEmployeesBasedOnAssignings>();

                // Get actual employees Based On Assignings by date
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesBasedOnAssignings = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x =>
                                                                                                                                                                 // (FromDateGr.Date >= x.AssigningStartDate.Date && FromDateGr.Date <= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date))
                                                                                                                                                                 (
                                                                                                                                                                  (FromDateGr >= x.AssigningStartDate && FromDateGr <= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date)) ||
                                                                                                                                                                  (ToDate >= x.AssigningStartDate && ToDate <= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date)) ||
                                                                                                                                                                  (FromDateGr >= x.AssigningStartDate && ToDate <= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date)) ||
                                                                                                                                                                  (FromDateGr <= x.AssigningStartDate && ToDate >= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date))
                                                                                                                                                                 )
                                                                                                                                                                 && OrganizationIDsList.Contains(x.OrganizationID.Value)).ToList();
                List<int> EmployeeCareerHistoryIDs = new List<int>();
                ActualEmployeesBasedOnAssignings.ForEach(x => EmployeeCareerHistoryIDs.Add(x.EmployeeCareerHistoryID.Value));

                List<OverTimesDetails> EmployeesDelegationsOfActualEmployeesList = new OverTimesDetailsDAL().GetEmployeesOverTimesByDate(FromDateGr, EmployeeCareerHistoryIDs);

                var query = EmployeesDelegationsOfActualEmployeesList.Select(y => new EmployeesOverTimesBasedOnAssigngingsDTO(y.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).EmployeeNameAr,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).OrganizationName,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).JobName,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).RankCategoryName,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).RankName,
                                                                      y.OverTimes.OverTimeStartDate,
                                                                      y.OverTimes.OverTimeEndDate,
                                                                      ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).Sorting
                                                                      ));

                return query.AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        internal OverTimesBLL MapOverTime(OverTimes OverTime)
        {
            try
            {
                OverTimesBLL OverTimeBLL = null;
                if (OverTime != null)
                {
                    OverTimeBLL = new OverTimesBLL()
                    {
                        OverTimeID = OverTime.OverTimeID,
                        OverTimeStartDate = OverTime.OverTimeStartDate,
                        OverTimeEndDate = OverTime.OverTimeEndDate,
                        Tasks = OverTime.Tasks,
                        WeekWorkHoursAvg = OverTime.WeekWorkHoursAvg,
                        FridayHoursAvg = OverTime.FridayHoursAvg.HasValue ? OverTime.FridayHoursAvg.Value : 0,
                        SaturdayHoursAvg = OverTime.SaturdayHoursAvg.HasValue ? OverTime.SaturdayHoursAvg.Value : 0,
                        Requester = OverTime.Requester,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(OverTime.CreatedByNav),
                        CreatedDate = OverTime.CreatedDate,
                        IsApproved = OverTime.IsApproved,
                        ApprovedBy = OverTime.ApprovedBy,
                        ApprovedDate = OverTime.ApprovedDate

                    };
                    OverTimeBLL.OverTimesDays = new List<OverTimesDaysBLL>();
                    OverTime.OverTimesDays.ToList().ForEach(c => OverTimeBLL.OverTimesDays.Add(new OverTimesDaysBLL() { OverTimeDay = c.OverTimeDay, OverTimeDayID = c.OverTimeDayID }));

                }
                return OverTimeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
