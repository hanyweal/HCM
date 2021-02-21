using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class HolidaysAttendanceDetailsBLL : CommonEntity, IEntity
    {
        public int HolidayAttendanceDetailID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public HolidaysAttendanceBLL HolidaysAttendance { get; set; }

        public Result Add(HolidaysAttendanceDetailsBLL HolidayAttendanceDetailBLL)
        {
            Result result = new Result();

            HolidaysAttendanceDetails HolidayAttendanceDetail = new HolidaysAttendanceDetails()
            {
                HolidayAttendanceID = HolidayAttendanceDetailBLL.HolidaysAttendance.HolidayAttendanceID,
                EmployeeCareerHistoryID = HolidayAttendanceDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID,
                CreateDate = DateTime.Now,
                CreatedBy = HolidayAttendanceDetailBLL.LoginIdentity.EmployeeCodeID
            };

            HolidaysAttendanceBLL HolidaysAttendanceBLL = new HolidaysAttendanceBLL().GetByHolidayAttendanceID(HolidayAttendanceDetailBLL.HolidaysAttendance.HolidayAttendanceID);
            List<HolidaysAttendanceBLL> HolidaysAttendanceBLLList = new HolidaysAttendanceBLL().GetByHolidaySettingID(HolidaysAttendanceBLL.HolidaySetting.HolidaySettingID);
            List<HolidaysAttendanceDetailsBLL> HolidaysAttendanceDetailsExists = new List<HolidaysAttendanceDetailsBLL>();
            
            HolidaysAttendanceBLLList.ForEach(c =>HolidaysAttendanceDetailsExists.AddRange(c.HolidaysAttendanceDetails.Where(x => x.EmployeeCareerHistory.EmployeeCareerHistoryID == HolidayAttendanceDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID).ToList()));
            if (HolidaysAttendanceDetailsExists.Count>0)
            {
                result.EnumType = typeof(HolidayAttendanceValidationEnum);
                result.EnumMember = HolidayAttendanceValidationEnum.RejectedBecauseEmployeeAlreadyExistOnAnotherRecord.ToString();
                return result;
            }


            this.HolidayAttendanceDetailID = new HolidaysAttendanceDetailsDAL().Insert(HolidayAttendanceDetail);
            if (this.HolidayAttendanceDetailID != 0)
            {
                result.Entity = null;
                result.EnumType = typeof(HolidayAttendanceValidationEnum);
                result.EnumMember = HolidayAttendanceValidationEnum.Done.ToString();
            }
            return result;
        }

        public Result Remove(int HolidayAttendanceDetailID)
        {
            try
            {
                Result result = null;
                new HolidaysAttendanceDetailsDAL().Delete(HolidayAttendanceDetailID, this.LoginIdentity.EmployeeCodeID);
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

        public List<HolidaysAttendanceDetailsBLL> GetHolidaysAttendanceDetailsByHolidayAttendanceID(int id)
        {
            List<HolidaysAttendanceDetails> HolidaysAttendanceDetailsList = new HolidaysAttendanceDetailsDAL().GetHolidaysAttendanceDetailsByHolidayAttendanceID(id);
            List<HolidaysAttendanceDetailsBLL> HolidaysAttendanceDetailsBLLList = new List<HolidaysAttendanceDetailsBLL>();
            if (HolidaysAttendanceDetailsList.Count > 0)
            {
                foreach (var item in HolidaysAttendanceDetailsList)
                {
                    HolidaysAttendanceDetailsBLLList.Add(new HolidaysAttendanceDetailsBLL()
                    {
                        HolidayAttendanceDetailID = item.HolidayAttendanceDetailID,
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item.EmployeesCareersHistory),
                        HolidaysAttendance =  new HolidaysAttendanceBLL() { HolidayAttendanceID=item.HolidayAttendanceID }
                    });
                    
                }
            }

            return HolidaysAttendanceDetailsBLLList;
        }

        public HolidaysAttendanceDetailsBLL GetByHolidayAttendanceDetailID(int HolidayAttendanceDetailID)
        {
            HolidaysAttendanceDetailsBLL HolidaysAttendanceDetailsBLL = null;
            HolidaysAttendanceDetails HolidayAttendanceDetail = new HolidaysAttendanceDetailsDAL().GetByHolidayAttendanceDetailID(HolidayAttendanceDetailID);
            if (HolidayAttendanceDetail != null)
            {
                HolidaysAttendanceDetailsBLL = new HolidaysAttendanceDetailsBLL().MapHolidayAttendanceDetail(HolidayAttendanceDetail);
            }
            return HolidaysAttendanceDetailsBLL;
        }

        internal HolidaysAttendanceDetailsBLL MapHolidayAttendanceDetail(HolidaysAttendanceDetails HolidayAttendanceDetail)
        {
            try
            {
                HolidaysAttendanceDetailsBLL HolidayAttendanceDetailBLL = null;
                if (HolidayAttendanceDetail != null)
                {
                    HolidayAttendanceDetailBLL = new HolidaysAttendanceDetailsBLL()
                    {
                        HolidayAttendanceDetailID = HolidayAttendanceDetail.HolidayAttendanceDetailID,
                        HolidaysAttendance = new HolidaysAttendanceBLL().GetByHolidayAttendanceID(HolidayAttendanceDetail.HolidayAttendanceID),
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(HolidayAttendanceDetail.EmployeesCareersHistory)
                    };
                }
                return HolidayAttendanceDetailBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}