using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HCMServices.Controllers
{
    //[CustomBasicAuthentication]
    public class EmployeesController : ApiController
    {
        // 1
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (EmployeeCodeBLL != null)
                return Request.CreateResponse(HttpStatusCode.OK, EmployeeCodeBLL);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee not found!");
        }

        // 2
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetByEmployeeCodeID/{EmployeeCodeID}")]
        public HttpResponseMessage GetByEmployeeCodeID(int EmployeeCodeID)
        {
            if (EmployeeCodeID == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(EmployeeCodeID);

            if (EmployeeCodeBLL != null)
                return Request.CreateResponse(HttpStatusCode.OK, EmployeeCodeBLL);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee not found!");
        }

        // 3
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployees/")]
        public HttpResponseMessage GetEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetEmployees()
                .Select(e => new
                {
                    EmployeeCodeID = e.EmployeeCodeID,
                    EmployeeCodeNo = e.EmployeeCodeNo,
                    Employee = new
                    {
                        EmployeeID = e.Employee.EmployeeID,
                        FirstNameAr = e.Employee.FirstNameAr,
                        MiddleNameAr = e.Employee.MiddleNameAr,
                        GrandFatherNameAr = e.Employee.GrandFatherNameAr,
                        LastNameAr = e.Employee.LastNameAr,
                        EmployeeMobileNo = e.Employee.EmployeeMobileNo
                    }
                }));
        }

        // 4
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetByEmployeeIDNo/{EmployeeIDNo}")]
        public HttpResponseMessage GetByEmployeeIDNo(string EmployeeIDNo)
        {
            //return new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
            if (string.IsNullOrEmpty(EmployeeIDNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesBLL emp = new EmployeesBLL().GetByEmployeeIDNo(EmployeeIDNo);

            if (emp != null)
                return Request.CreateResponse(HttpStatusCode.OK, emp);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 5
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetByEmployeeID/{EmployeeID}")]
        public HttpResponseMessage GetByEmployeeID(int EmployeeID)
        {
            //return new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
            if (EmployeeID == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesBLL emp = new EmployeesBLL().GetByEmployeeID(EmployeeID);

            if (emp != null)
                return Request.CreateResponse(HttpStatusCode.OK, emp);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 6
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployeesWithDetails/")]
        public HttpResponseMessage GetEmployeesWithDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetEmployeesWithDetails()
                .Select(e => new
                {
                    EmployeeCodeID = e.EmployeeCodeID,
                    EmployeeCodeNo = e.EmployeeCodeNo,
                    EmployeeCurrentJob = new
                    {

                        OrganizationJob = e.EmployeeCurrentJob != null ? new
                        {
                            Job = new { JobName = e.EmployeeCurrentJob.OrganizationJob.Job.JobName },
                            OrganizationStructure = new { OrganizationName = e.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName }
                        } : null
                    },
                    Employee = new
                    {
                        EmployeeID = e.Employee.EmployeeID,
                        EmployeeIDNo = e.Employee.EmployeeIDNo,
                        FirstNameAr = e.Employee.FirstNameAr,
                        MiddleNameAr = e.Employee.MiddleNameAr,
                        GrandFatherNameAr = e.Employee.GrandFatherNameAr,
                        LastNameAr = e.Employee.LastNameAr,
                        EmployeeMobileNo = e.Employee.EmployeeMobileNo
                    }
                }));

        }

        // 7
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetBasedOnAssigningsByOrganizationID/{OrganizationID}")]
        public List<EmployeesCodesBLL> GetBasedOnAssigningsByOrganizationID(int OrganizationID)
        {
            return new EmployeesCodesBLL().GetBasedOnAssigningsByOrganizationID(OrganizationID);

        }

        // 8
        [AllowAnonymous]
        [HttpPost, Route("api/Employees/EditEmployeeMobileNo/{EmployeeCodeNo}/{MobileNo}")]
        public HttpResponseMessage EditEmployeeMobileNo(string EmployeeCodeNo, string MobileNo)
        {
            //if(!new SMSBLL().ValidateMobileNum(MobileNo))
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Mobile No is not valid");

            EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
            if (EmployeeCodeBLL != null)
            {
                EmployeesBLL Employee = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo).Employee;
                Employee.EmployeeMobileNo = MobileNo;
                Employee.LoginIdentity = EmployeeCodeBLL;
                Employee.UpdateMobileNo();
                return Request.CreateResponse(HttpStatusCode.OK, "Employee mobile no has been updated successfuly");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee is not exist or Mobile No is not valid");
            }
        }

        // 9
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployeeImageByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetEmployeeImageByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                Byte[] bytes = new EmployeesBLL().GetEmployeePictureBytes(EmployeeCodeNo);

                result.Content = new StreamContent(new MemoryStream(bytes));
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                return result;
            }
            catch
            {
                throw;
            }
        }

        // 10
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetByOrganizationID/{OrganizationID}")]
        public List<EmployeesCodesBLL> GetByOrganizationID(int OrganizationID)
        {
            return new EmployeesCodesBLL().GetByOrganizationID(OrganizationID);
        }

        // 11
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetSalaryDetailsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetSalaryDetailsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL().GetSalaryDetails(EmployeeCodeNo).FirstOrDefault();

                var data = new
                {
                    Benefits = SalaryDetails.Benefits,
                    Deductions = SalaryDetails.Deductions,
                    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeNo = SalaryDetails.Employee.EmployeeCode.EmployeeCodeNo },
                    NetSalary = SalaryDetails.NetSalary
                };

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch
            {
                throw;
            }


            //try
            //{
            //    double TotalAllowances = 0;
            //    double BasicSalary = 0;
            //    double TransfareAllowance = 0;

            //    SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();
            //    SalaryDetails = SalaryDetails.GetSalaryDetailsByEmployeeCodeNo(EmployeeCodeNo);
            //    BasicSalary = SalaryDetails.BasicSalary;
            //    TransfareAllowance = SalaryDetails.TransfareAllowance;

            //    foreach (var item in SalaryDetails.EmployeesAllowances)
            //    {
            //        if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Fixed))
            //        {
            //            TotalAllowances = TotalAllowances + item.Allowance.AllowanceAmount;
            //        }
            //        else if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Percentage))
            //        {
            //            TotalAllowances = TotalAllowances + ((item.Allowance.AllowanceAmount / 100) * BasicSalary);
            //        }
            //    }

            //    var data = new
            //    {
            //        Salary = new
            //        {
            //            BasicSalary = BasicSalary,
            //            NetSalary = BasicSalary + TotalAllowances + TransfareAllowance,
            //        }
            //    };

            //    return Request.CreateResponse(HttpStatusCode.OK, data);
            //}
            //catch
            //{
            //    throw;
            //}
        }

        // 12
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEvaluationsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetEvaluationsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (EmployeeCodeNo == string.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            List<EmployeesEvaluationsBLL> empEvolution = new EmployeesEvaluationsBLL().GetEmployeeEvaluationsByEmployeeCodeNo(EmployeeCodeNo);

            if (empEvolution != null)
                return Request.CreateResponse(HttpStatusCode.OK, empEvolution);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 13
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetVacationBalanceByEmployeeCodeNo/{VacationType}/{EmployeeCodeNo}")]
        public HttpResponseMessage GetVacationBalanceByEmployeeCodeNo(VacationsTypesEnum VacationType, string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                BaseVacationsBLL Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                DateTime CurrentDate = DateTime.Now.Date;
                int MaturityYearFromCurrentDate = Globals.Calendar.GetUmAlQuraYear(CurrentDate);
                int MaturityMonthFromCurrentDate = Globals.Calendar.GetUmAlQuraMonth(CurrentDate);
                int MaturityDayFromCurrentDate = Globals.Calendar.GetUmAlQuraDay(CurrentDate);

                ((NormalVacationsBLL)Vacation).GetVacationBalances(emp.EmployeeCodeID, MaturityYearFromCurrentDate, MaturityMonthFromCurrentDate, MaturityDayFromCurrentDate);

                return Request.CreateResponse(HttpStatusCode.OK, (NormalVacationsBLL)Vacation);

                //return Request.CreateResponse(HttpStatusCode.OK, new 
                //{
                //    EmployeeCodeID = ((NormalVacationsBLL)Vacation).EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                //    EmployeeCodeNo = ((NormalVacationsBLL)Vacation).EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                //    EmployeeNameAr = ((NormalVacationsBLL)Vacation).EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,

                //    DeservedOldBalance = ((NormalVacationsBLL)Vacation).DeservedOldBalance,
                //    ConsumedOldBalance = ((NormalVacationsBLL)Vacation).ConsumedOldBalance,
                //    RemainingOldBalance = ((NormalVacationsBLL)Vacation).RemainingOldBalance,

                //    DeservedCurrentBalance = ((NormalVacationsBLL)Vacation).DeservedCurrentBalance,
                //    ConsumedCurrentBalance = ((NormalVacationsBLL)Vacation).ConsumedCurrentBalance,
                //    ExpiredCurrentBalance = ((NormalVacationsBLL)Vacation).ExpiredCurrentBalance,
                //    RemainingCurrentBalance = ((NormalVacationsBLL)Vacation).RemainingCurrentBalance,

                //    TotalDeservedBalance = ((NormalVacationsBLL)Vacation).TotalDeservedBalance,
                //    TotalConsumedBalance = ((NormalVacationsBLL)Vacation).TotalConsumedBalance,
                //    TotalRemainingBalance = ((NormalVacationsBLL)Vacation).TotalRemainingBalance,

                //    TotalConsumedSeparatedDays = ((NormalVacationsBLL)Vacation).TotalConsumedSeparatedDays,

                //    InAdvanceBalance = ((NormalVacationsBLL)Vacation).InAdvanceBalance,

                //    TotalAvailableVacationBalance = ((NormalVacationsBLL)Vacation).TotalAvailableVacationBalance,

                //}.ToXml());
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 14
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetCareerHistoryByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetCareerHistoryByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetCareerHistoryByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 15
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetDelegationsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetDelegationsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetDelegationsByEmployeeCodeID(emp.EmployeeCodeID).Select(x => new
                {
                    DelegationID = x.DelegationID,
                    DelegationTypeName = x.DelegationType.DelegationTypeName,
                    DelegationKindName = x.DelegationKind.DelegationKindName,
                    DelegationStartDate = x.DelegationStartDate,
                    DelegationEndDate = x.DelegationEndDate,
                    DelegationPeriod = x.DelegationPeriod
                }));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 16
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetOverTimesByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetOverTimesByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetOverTimesByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 17
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetPassengerOrdersByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetPassengerOrdersByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetPassengerOrdersByEmployeeCodeID(emp.EmployeeCodeID).Select(x => new
                {
                    PassengerOrderID = x.PassengerOrderID,
                    //PassengerOrderTypeName = x.PassengerOrderType.PassengerOrderTypeID == 1 ? "إنتداب" : "ابتعاث تدريبي",

                    //?????? needs to review
                    //StartDate = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationStartDate : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipStartDate,
                    //EndDate = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationEndDate : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipEndDate,
                    //Reason = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationReason : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipReason,
                    TravellingDate = x.TravellingDate,
                    Going = x.Going,
                    Returning = x.Returning,
                }));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 18
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetAssigningsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetAssigningsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetAssigningsByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 19
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetLendersByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetLendersByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetLendersByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 20
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetGovernmentFundsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetGovernmentFundsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetGovernmentFundsByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 21
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetAllowancesByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetAllowancesByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetActiveAllownacessByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 22
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetInternshipScholarshipsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetInternshipScholarshipsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetInternshipScholarshipsByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 23
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetScholarshipsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetScholarshipsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetScholarshipsByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 24
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployeeExperiencesByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetEmployeeExperiencesByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetExperiencesByEmployeeCodeID(emp.EmployeeCodeID));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 25
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployeeQualificationsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetEmployeeQualificationsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetQualificationsByEmployeeCodeID(emp.EmployeeCodeID, 0));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 26
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetVacationsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetVacationsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetVacationsDetailsByEmployeeCodeID(emp.EmployeeCodeID).Select(x => new
                {
                    VacationID = x.Vacation.VacationID,
                    VacationDetailID = x.VacationDetailID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    VacationPeriod = x.VacationPeriod,
                    VacationTypeName = x.Vacation.VacationTypeName,
                    VacationActionName = x.VacationAction.VacationActionName,
                }).OrderBy(x => x.FromDate));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }

        // 27
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetPromotionRecordsByEmployeeCodeNo/{EmployeeCodeNo}")]
        public HttpResponseMessage GetPromotionRecordsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            if (string.IsNullOrEmpty(EmployeeCodeNo))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            EmployeesCodesBLL emp = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

            if (emp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetPromotionRecordsByEmployeeCodeID(emp.EmployeeCodeID).Select(x => new
                {
                    Year = x.PromotionRecord.PromotionPeriod.Year.MaturityYear,
                    PeriodName = x.PromotionRecord.PromotionPeriod.Period.PeriodName,
                    PromotionRecordNo = x.PromotionRecord.PromotionRecordNo,
                    PromotionRecordDate = x.PromotionRecord.PromotionRecordDate,
                    RankName = x.CurrentEmployeeCareer.OrganizationJob.Rank.RankName,
                    JobName = x.CurrentEmployeeCareer.OrganizationJob.Job.JobName,
                    JobNo = x.CurrentEmployeeCareer.OrganizationJob.JobNo,
                    IsPromoted = x.NewEmployeeCareer,
                    IsRemoval = x.IsRemovedAfterIncluding,
                    PromotionCandidateAddedWayName = x.PromotionCandidateAddedWay.PromotionCandidateAddedWayName,
                }));
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee not found!");
        }
        //28
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetAllEmployeesWithDetails/")]
        public HttpResponseMessage GetAllEmployeesWithDetails()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new EmployeesCodesBLL().GetAllEmployeesWithDetails()
                .Select(e => new
                {
                    EmployeeCodeID = e.EmployeeCodeID,
                    EmployeeCodeNo = e.EmployeeCodeNo,
                    EmployeeCurrentJob = new
                    {

                        OrganizationJob = e.EmployeeCurrentJob != null ? new
                        {
                            Job = new { JobName = e.EmployeeCurrentJob.OrganizationJob.Job.JobName },
                            OrganizationStructure = new { OrganizationName = e.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName }
                        } : null
                    },
                    Employee = new
                    {
                        EmployeeID = e.Employee.EmployeeID,
                        EmployeeIDNo = e.Employee.EmployeeIDNo,
                        FirstNameAr = e.Employee.FirstNameAr,
                        MiddleNameAr = e.Employee.MiddleNameAr,
                        GrandFatherNameAr = e.Employee.GrandFatherNameAr,
                        LastNameAr = e.Employee.LastNameAr,
                        EmployeeMobileNo = e.Employee.EmployeeMobileNo
                    }
                }));

        }

        // 29
        [AllowAnonymous]
        [HttpGet, Route("api/Employees/GetEmployeesEndOfServices")]
        public HttpResponseMessage GetEmployeesEndOfServices()
        {
            List<EndOfServicesBLL> lstEos = new EndOfServicesBLL().GetEndOfServices();

            if (lstEos != null)
                return Request.CreateResponse(HttpStatusCode.OK, lstEos);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Employees EndOfServices no data!");
        }

    }
}
