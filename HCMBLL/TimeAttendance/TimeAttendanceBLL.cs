using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using TimeAttendanceDTO;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class TimeAttendanceBLL
    {
        private HttpClient client;
        public TimeAttendanceBLL()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["TimeAttendanceWebApiURL"])
            };
        }
        public string EmployeeCode { get; set; }

        public DateTime Date { get; set; }

        public static DateTime FromDate
        {
            get
            {
                return DateTime.Now.AddYears(-7).Date;
            }
        }

        public static DateTime ToDate { get; set; }

        private static List<TimeAttendanceBLL> _AbsenceDays { get; set; }

        /// <summary>
        /// Change Date: 26-04-2020 
        /// User Requested to stop Integration b/w timeattendance and HCM because In TimeAttendance data is not 100% for absents.
        /// </summary>
        public static List<TimeAttendanceBLL> AbsenceDays
        {
            get
            {
                return new List<TimeAttendanceBLL>();
            }
            //get
            //{
            //    if (_AbsenceDays == null)
            //    {
            //        _AbsenceDays = new TimeAttendanceBLL().GetEmployeesAbsences(FromDate, ToDate);
            //        return _AbsenceDays;
            //    }
            //    else
            //        return _AbsenceDays;
            //}
        }

        /// <summary>
        /// Change Date: 26-04-2020 
        /// User Requested to stop Integration b/w timeattendance and HCM because In TimeAttendance data is not 100% for absents.
        /// </summary>
        public List<TimeAttendanceBLL> GetEmployeesAbsences(DateTime FromDate, DateTime ToDate)
        {
            //Result result = new Result();

            //// Add an accept header for json format
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //string FromDateGreg = Globals.Calendar.UmAlquraToGreg(FromDate.ToString("yyyy-MM-dd"));
            //string ToDateGreg = Globals.Calendar.UmAlquraToGreg(ToDate.ToString("yyyy-MM-dd"));
            //HttpResponseMessage response = client.GetAsync(string.Format("TimeAttendance/GetEmployeesAbsencesResult/{0}/{1}", FromDateGreg, ToDateGreg)).Result;
            //List<TimeAttendanceBLL> res = JsonConvert.DeserializeObject<List<TimeAttendanceBLL>>(response.Content.ReadAsStringAsync().Result);

            //return res;
            return new List<TimeAttendanceBLL>();
        }

        /// <summary>
        /// Change Date: 26-04-2020 
        /// User Requested to stop Integration b/w timeattendance and HCM because In TimeAttendance data is not 100% for absents.
        /// </summary>
        public List<TimeAttendanceBLL> GetAbsenceDaysByEmployeeCodeNo(string EmployeeCodeNo, DateTime FromDate, DateTime ToDate)
        {
            //Result result = new Result();

            //// Add an accept header for json format
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //string FromDateGreg = Globals.Calendar.UmAlquraToGreg(FromDate.ToString("yyyy-MM-dd"));
            //string ToDateGreg = Globals.Calendar.UmAlquraToGreg(ToDate.ToString("yyyy-MM-dd"));
            //HttpResponseMessage response = client.GetAsync(string.Format("TimeAttendance/GetEmployeeAbsenceResult/{0}/{1}/{2}", EmployeeCodeNo, FromDateGreg, ToDateGreg)).Result;
            //List<TimeAttendanceBLL> res = JsonConvert.DeserializeObject<List<TimeAttendanceBLL>>(response.Content.ReadAsStringAsync().Result);

            ////res.ForEach(x => x.EmployeeCodeNo = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo));

            //return res;
            return new List<TimeAttendanceBLL>();
        }

        //public List<TimeAttendanceBLL> GetAbsenceDaysByEmployeeCodeNo(List<TimeAttendanceBLL> AbsenceDaysList, string EmployeeCodeNo, DateTime FromDate, DateTime ToDate)
        //{
        //    return AbsenceDaysList.Where(x => x.EmployeeCode == EmployeeCodeNo && x.Date <= FromDate && x.Date >= ToDate).ToList();
        //}

        /// <summary>
        /// Change Date: 26-04-2020 
        /// User Requested to stop Integration b/w timeattendance and HCM because In TimeAttendance data is not 100% for absents.
        /// </summary>
        public List<EmployeesAbsencesCountDTO> GetEmployeesAbsencesDaysCount(DateTime FromDate, DateTime ToDate)
        {
            //Result result = new Result();

            //// Add an accept header for json format
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //string FromDateGreg = Globals.Calendar.UmAlquraToGreg(FromDate.ToString("yyyy-MM-dd"));
            //string ToDateGreg = Globals.Calendar.UmAlquraToGreg(ToDate.ToString("yyyy-MM-dd"));
            //HttpResponseMessage response = client.GetAsync(string.Format("TimeAttendance/GetEmployeesAbsencesDaysCountResult/{0}/{1}", FromDateGreg, ToDateGreg)).Result;
            //List<EmployeesAbsencesCountDTO> res = JsonConvert.DeserializeObject<List<EmployeesAbsencesCountDTO>>(response.Content.ReadAsStringAsync().Result);

            //return res;
            return new List<EmployeesAbsencesCountDTO>();
        }

        /// <summary>
        /// Change Date: 26-04-2020 
        /// User Requested to stop Integration b/w timeattendance and HCM because In TimeAttendance data is not 100% for absents.
        /// </summary>
        public EmployeesAbsencesCountDTO GetEmployeesAbsenceDaysCountByEmployeeCodeNo(List<EmployeesAbsencesCountDTO> AbsenceDaysList, string EmployeeCodeNo)
        {
            //return AbsenceDaysList.Where(x => x.EmployeeCode == EmployeeCodeNo).FirstOrDefault();
            return new EmployeesAbsencesCountDTO();
        }

        /// <summary>
        ///  To send new vacation data of employee to Time Attendance App
        /// </summary>
        /// <param name="EmployeeCodeNo"></param>
        /// <param name="VacationType"></param>
        /// <param name="VacationStartDate"></param>
        /// <param name="VacationEndDate"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="CreatedDate"></param>
        /// <param name="VacationID"></param>
        /// <returns></returns>
        public Result AddEmployeeVacation(int VacationID,string EmployeeCodeNo, VacationsTypesEnum VacationType, DateTime VacationStartDate, DateTime VacationEndDate, string CreatedBy, DateTime CreatedDate)
        {
            try
            {
                Result result = new Result();
                result.EnumType = typeof(VacationsValidationEnum);
                AddEmployeeVacation EmployeeVacationDTO = new AddEmployeeVacation()
                { 
                    EmployeeCode = EmployeeCodeNo,
                    HCMVacationTypeID = (int)VacationType,
                    HCMVacationID = VacationID,
                    StartDate = VacationStartDate,
                    EndDate = VacationEndDate,
                    UserEmployeeCodeNo = CreatedBy,
                    CreatedDate = CreatedDate,
                    ApprovedByEmployeeCodeNo = CreatedBy,
                    ApprovedDate = CreatedDate
                };
                var PostTask = client.PostAsJsonAsync<AddEmployeeVacation>("EmployeeVacation", EmployeeVacationDTO).Result;

                if (PostTask.IsSuccessStatusCode)
                    result.EnumMember = VacationsValidationEnum.Done.ToString();
                else
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To send updated vacation data of employee to Time Attendance App
        /// </summary>
        /// <param name="VacationStartDate"></param>
        /// <param name="VacationEndDate"></param>
        /// <param name="UpdatedBy"></param>
        /// <param name="VacationID"></param>
        /// <returns></returns>
        public Result EditEmployeeVacation(int VacationID,DateTime VacationStartDate, DateTime VacationEndDate, string UpdatedBy)
        {
            try
            {
                Result result = new Result();
                result.EnumType = typeof(VacationsValidationEnum);
                EditEmployeeVacation EmployeeVacationDTO = new EditEmployeeVacation()
                {
                    HCMVacationID = VacationID,
                    StartDate = VacationStartDate,
                    EndDate = VacationEndDate,
                    UserEmployeeCodeNo = UpdatedBy,
                };
                var PutTask = client.PutAsJsonAsync<EditEmployeeVacation>("EmployeeVacation", EmployeeVacationDTO).Result;

                if (PutTask.IsSuccessStatusCode)
                    result.EnumMember = VacationsValidationEnum.Done.ToString();
                else
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To send deleted vacation of employee to Time Attendance App
        /// </summary>
        /// <param name="VacationID"></param>
        /// <returns></returns>
        public Result DeleteEmployeeVacation(int VacationID, string DeletedBy)
        {
            try
            {
                Result result = new Result();
                result.EnumType = typeof(VacationsValidationEnum);
                EditEmployeeVacation EmployeeVacationDTO = new EditEmployeeVacation()
                {
                    HCMVacationID = VacationID,
                    UserEmployeeCodeNo = DeletedBy,
                };
                var DeleteTask = client.DeleteAsync("EmployeeVacation/ " + VacationID + "/" + DeletedBy);

                if (DeleteTask.Result.IsSuccessStatusCode)
                    result.EnumMember = VacationsValidationEnum.Done.ToString();
                else
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}