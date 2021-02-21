using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class CareerHistoryController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("CareerHistory/GetOrganizationsJobs/{JobNo}/{OrganizationName}/{JobName}/{RankName}")]
        public JsonResult GetOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName)
        {
            var data = new OrganizationsJobsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetActiveOrganizationsJobs(JobNo, OrganizationName, JobName, RankName, "", "", out TotalRecordsOut, out RecFilterOut).Select(x => new
            {
                OrganizationJobID = x.OrganizationJobID,
                OrganizationName = x.OrganizationStructure.OrganizationName,
                JobName = x.Job.JobName,
                RankName = x.Rank.RankName,
                JobNo = x.JobNo,
                x.IsVacant
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCareersHistoryTypes()
        {
            return Json(new
            {
                data = new CareersHistoryTypesBLL().GetCareersHistoryTypes()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCareersDegrees()
        {
            return Json(new
            {
                data = new CareersDegreesBLL().GetCareersDegrees()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Task 221    :   Break last assigning when new Career history record created.
        /// Dated       :   08-12-2020 
        /// </summary>
        /// <param name="CareerHistoryVM"></param>
        /// <returns></returns>
        public HttpResponseMessage Save(CareerHistoryViewModel CareerHistoryVM)
        {
            if (CareerHistoryVM.EmployeeCodeID == 0)
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeText);
            }
            else
            {
                //EmployeesCareersHistoryBLL LastActiveCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(CareerHistoryVM.EmployeeCodeID);

                EmployeesCareersHistoryBLL Obj = new EmployeesCareersHistoryBLL()
                {
                    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = CareerHistoryVM.EmployeeCodeID },
                    EmployeeCareerHistoryID = CareerHistoryVM.EmployeeCareerHistoryID,
                    JoinDate = (DateTime)CareerHistoryVM.JoinDate,
                    OrganizationJob = new OrganizationsJobsBLL() { OrganizationJobID = CareerHistoryVM.OrganizationJobID },
                    CareerDegree = new CareersDegreesBLL() { CareerDegreeID = CareerHistoryVM.CareerDegreeID },
                    CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = CareerHistoryVM.CareerHistoryTypeID },
                };
                Result result = null;
                if (CareerHistoryVM.EmployeeCareerHistoryID != 0)
                {
                    Obj.LoginIdentity = UserIdentity;
                    result = Obj.Update();
                }
                else
                {
                    Obj.LoginIdentity = UserIdentity;
                    result = Obj.Add();
                }

                if ((System.Type)result.EnumType == typeof(CareersHistoryValidationEnum))
                {
                    if (result.EnumMember == CareersHistoryValidationEnum.RejectedBecauseOfAlreadyHiringBefore.ToString())
                        throw new CustomException(Resources.Globalization.ValidationHiringAlreadyExistText);
                    else if (result.EnumMember == CareersHistoryValidationEnum.RejectedHiringDateUpdatingBecauseOfAlreadyActionsBefore.ToString())
                        throw new CustomException(Resources.Globalization.ValidationHiringJoinDateUpdatingText);
                    else if (result.EnumMember == CareersHistoryValidationEnum.RejectedBecauseOfExistsInPromotionsRecordsEmployees.ToString())
                        throw new CustomException(Resources.Globalization.RecordRalatedWithOtherDataCanNotUpdateText);
                    else if (result.EnumMember == CareersHistoryValidationEnum.RejectedBecauseOfJoinDateMustBeLessThanHiringRecordJoinDate.ToString())
                        throw new CustomException(Resources.Globalization.ValidationJoinDateMustBeLessThanHiringRecordJoinDateText);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [Route("CareerHistory/UpdateCareerDegree/{EmployeeCareerHistoryID}/{CareerDegreeID}")]
        public JsonResult UpdateCareerDegree(int EmployeeCareerHistoryID, int CareerDegreeID)
        {
            EmployeesCareersHistoryBLL EmployeesCareersHistory = new EmployeesCareersHistoryBLL();
            EmployeesCareersHistory.EmployeeCareerHistoryID = EmployeeCareerHistoryID;
            EmployeesCareersHistory.CareerDegree = new CareersDegreesBLL() { CareerDegreeID = CareerDegreeID };
            EmployeesCareersHistory.LoginIdentity = this.UserIdentity;

            Result result = EmployeesCareersHistory.UpdateCareerDegree();
            return Json(new { data = EmployeeCareerHistoryID }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/DeleteEmployeeCareerHistoryByEmployeeCareerHistoryID/{EmployeeCareerHistoryID}/{EmployeeCodeID}")]
        public JsonResult DeleteEmployeeCareerHistoryByEmployeeCareerHistoryID(int EmployeeCareerHistoryID, int EmployeeCodeID)
        {
            EmployeesCareersHistoryBLL EmployeesCareersHistory = new EmployeesCareersHistoryBLL();
            EmployeesCareersHistory.LoginIdentity = this.UserIdentity;
            EmployeesCareersHistory.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID };
            Result result = EmployeesCareersHistory.Remove(EmployeeCareerHistoryID);
            return Json(new
            {
                data = EmployeesCareersHistory
            }, JsonRequestBehavior.AllowGet);

        }
    }
}