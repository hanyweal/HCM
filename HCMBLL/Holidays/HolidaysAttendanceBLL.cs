using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMBLL.Enums;
using HCMDAL;

namespace HCMBLL
{
    public class HolidaysAttendanceBLL : CommonEntity, IEntity
    {
        public int HolidayAttendanceID { get; set; }

        public HolidaysSettingsBLL HolidaySetting { get; set; }

        public OrganizationsStructuresBLL Organization { get; set; }

        public List<HolidaysAttendanceDetailsBLL> HolidaysAttendanceDetails { get; set; }


        public Result Add()
        {
            Result result;
            // validate employees PassengerOrder limit
            if (this.HolidaysAttendanceDetails == null || this.HolidaysAttendanceDetails.Count <= 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(HolidayAttendanceValidationEnum);
                result.EnumMember = HolidayAttendanceValidationEnum.RejectedBecauseEmployeeRequired.ToString();

                return result;
            }

            HolidaysAttendance HolidayAttendance = new HolidaysAttendance();
            HolidayAttendance.OrganizationID = this.Organization.OrganizationID;
            HolidayAttendance.HolidaySettingID = this.HolidaySetting.HolidaySettingID;
            HolidayAttendance.CreatedDate = DateTime.Now;
            HolidayAttendance.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.HolidaysAttendanceDetails.ForEach(c => HolidayAttendance.HolidaysAttendanceDetails.Add(new HolidaysAttendanceDetails()
            {
                CreatedBy = this.LoginIdentity.EmployeeCodeID,
                CreateDate = DateTime.Now,
                EmployeeCareerHistoryID = c.EmployeeCareerHistory.EmployeeCareerHistoryID
            }));

            #region Check if any employee in holidayAttendanceDetails repeated in the same holidaySetting
            List<HolidaysAttendanceBLL> HolidaysAttendanceList = new HolidaysAttendanceBLL().GetByHolidaySettingID(this.HolidaySetting.HolidaySettingID);
            List<HolidaysAttendanceDetailsBLL> HolidaysAttendanceDetailsList = new List<HolidaysAttendanceDetailsBLL>();
            HolidaysAttendanceList.ForEach(x=> x.HolidaysAttendanceDetails.ForEach(c =>HolidaysAttendanceDetailsList.AddRange(this.HolidaysAttendanceDetails.Where(s => s.EmployeeCareerHistory.EmployeeCareerHistoryID==c.EmployeeCareerHistory.EmployeeCareerHistoryID).ToList())));
            if (HolidaysAttendanceDetailsList.Count > 0)
            {
                result = new Result();
                result.Entity = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(HolidaysAttendanceDetailsList.FirstOrDefault().EmployeeCareerHistory.EmployeeCareerHistoryID);
                result.EnumMember = HolidayAttendanceValidationEnum.RejectedBecauseEmployeeAlreadyExistOnAnotherRecord.ToString();
                result.EnumType = typeof(HolidayAttendanceValidationEnum);
                return result;
            }

            #endregion

            this.HolidayAttendanceID = new HolidaysAttendanceDAL().Insert(HolidayAttendance);
            result = new Result();
           // result.Entity = this;
            result.EnumMember = HolidayAttendanceValidationEnum.Done.ToString();
            result.EnumType = typeof(HolidayAttendanceValidationEnum);
            return result;
        }

        public HolidaysAttendanceBLL GetByHolidayAttendanceID(int HolidayAttendanceID)
        {
            HolidaysAttendanceBLL HolidaysAttendanceBLL = null;
            HolidaysAttendance HolidayAttendance = new HolidaysAttendanceDAL().GetByHolidayAttendanceID(HolidayAttendanceID);
            if (HolidayAttendance != null)
            {
                HolidaysAttendanceBLL = new HolidaysAttendanceBLL().MapHolidayAttendance(HolidayAttendance);
            }
            return HolidaysAttendanceBLL;
        }

        public List<HolidaysAttendanceBLL> GetByHolidaySettingID(int HolidaySettingID)
        {
            List<HolidaysAttendanceBLL> HolidayAttendanceBLLList = new List<HolidaysAttendanceBLL>();
            List<HolidaysAttendance> HolidaysAttendance = new HolidaysAttendanceDAL().GetByHolidaySettingID(HolidaySettingID);
            foreach (var item in HolidaysAttendance)
            {
                HolidayAttendanceBLLList.Add(new HolidaysAttendanceBLL().MapHolidayAttendance(item));
            }
            return HolidayAttendanceBLLList;
        }

        public List<HolidaysAttendanceBLL> GetHolidaysAttendance()
        {
            List<HolidaysAttendanceBLL> HolidayAttendanceBLLList = new List<HolidaysAttendanceBLL>();
            List<HolidaysAttendance> HolidaysAttendance = new HolidaysAttendanceDAL().GetHolidaysAttendance();
            foreach (var item in HolidaysAttendance)
            {
                HolidayAttendanceBLLList.Add(new HolidaysAttendanceBLL().MapHolidayAttendance(item));
            }
            return HolidayAttendanceBLLList;
        }

        public Result Remove()
        {
            try
            {
                Result result = null;
                new HolidaysAttendanceDAL().Delete(new HolidaysAttendance() { HolidayAttendanceID = HolidayAttendanceID }, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(HolidayAttendanceValidationEnum),
                    EnumMember = HolidayAttendanceValidationEnum.Done.ToString()
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
            List<HolidaysAttendanceDetailsBLL> HolidayAttendanceDetailBLLList = new HolidaysAttendanceDetailsBLL().GetHolidaysAttendanceDetailsByHolidayAttendanceID(this.HolidayAttendanceID);

            // validate employees  
            if (HolidayAttendanceDetailBLLList == null || HolidayAttendanceDetailBLLList.Count <= 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(HolidayAttendanceValidationEnum);
                result.EnumMember = HolidayAttendanceValidationEnum.RejectedBecauseEmployeeRequired.ToString();

                return result;
            }
            else
            {
                HolidaysAttendance HolidayAttendance = new HolidaysAttendance()
                {
                    HolidayAttendanceID = this.HolidayAttendanceID,
                    HolidaySettingID=this.HolidaySetting.HolidaySettingID,
                    OrganizationID=this.Organization.OrganizationID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                new HolidaysAttendanceDAL().Update(HolidayAttendance);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(HolidayAttendanceValidationEnum),
                    EnumMember = HolidayAttendanceValidationEnum.Done.ToString()
                };
            }

            return result;
        }

        public List<HolidaysAttendanceBLL> GetByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
           // List<HolidaysAttendanceBLL> HolidaysAttendanceBll = new EmployeesCodesBLL();//.GetHolidaysAttendanceByEmployeeCodeID(EmployeeCodeID);
            //List<HolidaysAttendanceBLL> HolidaysAttendanceWithDaysBll = new List<HolidaysAttendanceBLL>();
            //foreach (var HolidayAttendance in HolidaysAttendanceBll)
            //{
            //    if (HolidayAttendance.HolidaysAttendanceDays.Exists(c => c.HolidayAttendanceDay >= StartDate && c.HolidayAttendanceDay <= EndDate))
            //    {
            //        HolidaysAttendanceWithDaysBll.Add(HolidayAttendance);
            //    }
            //}
            //return HolidaysAttendanceWithDaysBll;
            return new List<HolidaysAttendanceBLL>();

        }

        internal HolidaysAttendanceBLL MapHolidayAttendance(HolidaysAttendance HolidayAttendance)
        {
            try
            {
                HolidaysAttendanceBLL HolidayAttendanceBLL = null;
                if (HolidayAttendance != null)
                {
                    HolidayAttendanceBLL = new HolidaysAttendanceBLL()
                    {
                        HolidayAttendanceID = HolidayAttendance.HolidayAttendanceID,
                        HolidaySetting = new HolidaysSettingsBLL().MapHolidaySetting(HolidayAttendance.HolidaysSettings),
                        Organization = new OrganizationsStructuresBLL().MapOrganization(HolidayAttendance.OrganizationsStructures),
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(HolidayAttendance.CreatedByNav),
                        CreatedDate = HolidayAttendance.CreatedDate,
                     
                    };
                    HolidayAttendanceBLL.HolidaysAttendanceDetails = new HolidaysAttendanceDetailsBLL().GetHolidaysAttendanceDetailsByHolidayAttendanceID(HolidayAttendance.HolidayAttendanceID);
 
                }
                return HolidayAttendanceBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
