using HCM.Classes;
using HCMBLL;
using HCMBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class ManagerDashboardController : BaseEServicesController
    {
        public ActionResult Index()
        {
            Session["UserID"] = 0;
            return View();
        }

        [HttpGet]
        public JsonResult GetManagerOrganizationsWithChilds()
        {
            List<OrganizationsStructuresBLL> orgList = new OrganizationsStructuresBLL().GetManagerOrganizationsWithChildsAsTree(this.WindowsUserIdentity);
            return Json(new { data = orgList }, JsonRequestBehavior.AllowGet);
        }

        #region ActualEmployees
        [HttpGet]
        [Route("{controller}/GetActualEmployeesAsRanksCategories/{OrganizationID}")]
        public JsonResult GetActualEmployeesAsRanksCategories(int OrganizationID)
        {
            IQueryable<AssigngingsDTO> ActualEmployees;
            ActualEmployees = new InternalAssigningBLL().GetActualEmployeesBasedOnAssigningsAsRanksCategories(OrganizationID);
            Session["ActualEmployeesDetails"] = ActualEmployees;

            var ActualEmployeessByRanksCategories = ActualEmployees.OrderBy(x=>x.RankName).GroupBy(x => x.RankCategoryName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = ActualEmployeessByRanksCategories }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetActualEmployeesAsRanksCategoriesDetails()
        {
            IQueryable<AssigngingsDTO> ActualEmployeesDetails = (IQueryable<AssigngingsDTO>)Session["ActualEmployeesDetails"];
            var ActualEmployeesByRanksCategoriesDetails = ActualEmployeesDetails.Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x=> x.Sorting);

            return Json(new { data = ActualEmployeesByRanksCategoriesDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetActualEmployeesAsRanks/{RankCategoryName}")]
        public JsonResult GetActualEmployeesAsRanks(string RankCategoryName)
        {
            IQueryable<AssigngingsDTO> ActualEmployeesDetails = (IQueryable<AssigngingsDTO>)Session["ActualEmployeesDetails"];
            var ActualEmployeesDetailsAsRanks = ActualEmployeesDetails
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankCategoryName == RankCategoryName)
                     .GroupBy(x => x.RankName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = ActualEmployeesDetailsAsRanks }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetActualEmployeesAsRanksDetails/{RankCategoryName}")]
        public JsonResult GetActualEmployeesAsRanksDetails(string RankCategoryName)
        {
            IQueryable<AssigngingsDTO> ActualEmployeesDetails = (IQueryable<AssigngingsDTO>)Session["ActualEmployeesDetails"];
            var ActualEmployeesByRanksDetails = ActualEmployeesDetails.Where(x => x.RankCategoryName == RankCategoryName).OrderBy(x => x.RankName).Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = ActualEmployeesByRanksDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetActualEmployeesAsDetails/{RankName}")]
        public JsonResult GetActualEmployeesAsDetails(string RankName)
        {
            IQueryable<AssigngingsDTO> ActualEmployees = (IQueryable<AssigngingsDTO>)Session["ActualEmployeesDetails"];
            var ActualEmployeesByDetails = ActualEmployees
                     .Where(x => x.RankName == RankName)
                     .Select(y => new
                     {
                         EmployeeCodeNo = y.EmployeeCodeNo,
                         EmployeeNameAr = y.EmployeeNameAr,
                         OrganizationName = y.OrganizationName,
                         JobName = y.JobName,
                         RankCategoryName = y.RankCategoryName,
                         RankName = y.RankName,
                         Sorting = y.Sorting
                     }).OrderByDescending(x => x.RankCategoryName)
                       .ThenByDescending(x => x.Sorting);

            return Json(new { data = ActualEmployeesByDetails }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EmployeesQualifications
        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesQualificationsAsRanksCategories/{QualificationDegreeID}/{QualificationID}/{GeneralSpecializationID}/{OrganizationID}")]
        public JsonResult GetEmployeesQualificationsAsRanksCategories(int QualificationDegreeID, int QualificationID, int GeneralSpecializationID, int OrganizationID)
        {
            IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> Qualifications;
            Qualifications = new EmployeesQualificationsBLL().GetQualificationsBasedOnAssigningsAsRanksCategoriesDetails(QualificationDegreeID, QualificationID, GeneralSpecializationID, OrganizationID);
            Session["EmployeesQualificationsDetails"] = Qualifications;

            var QualificationsByRanksCategories = Qualifications
                     .GroupBy(x => x.RankCategoryName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = QualificationsByRanksCategories }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeesQualificationsAsRanksCategoriesDetails()
        {
            IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> QualificationsByRanksCategories = (IQueryable<EmployeesQualificationBasedOnAssigngingsDTO>)Session["EmployeesQualificationsDetails"];
            var QualificationsByRanksCategoriesDetails = QualificationsByRanksCategories.Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                QualificationDegreeName = y.QualificationDegreeName,
                QualificationName = y.QualificationName,
                GeneralSpecializationName = y.GeneralSpecializationName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = QualificationsByRanksCategoriesDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesQualificationsAsRanks/{RankCategoryName}")]
        public JsonResult GetEmployeesQualificationsAsRanks(string RankCategoryName)
        {
            IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> Qualifications = (IQueryable<EmployeesQualificationBasedOnAssigngingsDTO>)Session["EmployeesQualificationsDetails"];
            var QualificationsAsRanks = Qualifications
                     .OrderByDescending(x => x.Sorting)
                     .Where(x=> x.RankCategoryName == RankCategoryName)
                     .GroupBy(x => x.RankName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = QualificationsAsRanks }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesQualificationsAsRanksDetails/{RankCategoryName}")]
        public JsonResult GetEmployeesQualificationsAsRanksDetails(string RankCategoryName)
        {
            IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> Qualifications = (IQueryable<EmployeesQualificationBasedOnAssigngingsDTO>)Session["EmployeesQualificationsDetails"];
            var QualificationsByRanksDetails = Qualifications.Where(x => x.RankCategoryName == RankCategoryName).Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                QualificationDegreeName = y.QualificationDegreeName,
                QualificationName = y.QualificationName,
                GeneralSpecializationName = y.GeneralSpecializationName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = QualificationsByRanksDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesQualificationsAsDetails/{RankName}")]
        public JsonResult GetEmployeesQualificationsAsDetails(string RankName)
        {
            IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> Qualification = (IQueryable<EmployeesQualificationBasedOnAssigngingsDTO>)Session["EmployeesQualificationsDetails"];
            var QualificationsAsDetails = Qualification
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankName == RankName)
                     .Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                QualificationDegreeName = y.QualificationDegreeName,
                QualificationName = y.QualificationName,
                GeneralSpecializationName = y.GeneralSpecializationName
            });

            return Json(new { data = QualificationsAsDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeesVacations
        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesVacationsAsRanksCategories/{VacationTypeID}/{VacationDate}/{OrganizationID}")]
        public JsonResult GetEmployeesVacationsAsRanksCategories(int VacationTypeID, DateTime VacationDate, int OrganizationID)
        {
            IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> Vacations;
            Vacations = new BaseVacationsBLL().GetEmployeesVacationsBasedOnAssigningsAsRanksCategories(VacationTypeID, VacationDate, OrganizationID);
            Session["EmployeesVacationsDetails"] = Vacations;

            var VacationsByRanksCategories = Vacations
                     .GroupBy(x => x.RankCategoryName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = VacationsByRanksCategories }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeesVacationsAsRanksCategoriesDetails()
        {
            IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> VacationsByRanksCategories = (IQueryable<EmployeesVacationsBasedOnAssigngingsDTO>)Session["EmployeesVacationsDetails"];
            var VacationsByRanksCategoriesDetails = VacationsByRanksCategories.Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                VacationStartDate = y.VacationStartDate,
                VacationPeriod = y.VacationPeriod,
                VacationTypeName = y.VacationTypeName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = VacationsByRanksCategoriesDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesVacationsAsRanks/{RankCategoryName}")]
        public JsonResult GetEmployeesVacationsAsRanks(string RankCategoryName)
        {
            IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> Vacations = (IQueryable<EmployeesVacationsBasedOnAssigngingsDTO>)Session["EmployeesVacationsDetails"];
            var VacationsAsRanks = Vacations
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankCategoryName == RankCategoryName)
                     .GroupBy(x => x.RankName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = VacationsAsRanks }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesVacationsAsRanksDetails/{RankCategoryName}")]
        public JsonResult GetEmployeesVacationsAsRanksDetails(string RankCategoryName)
        {
            IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> Vacations = (IQueryable<EmployeesVacationsBasedOnAssigngingsDTO>)Session["EmployeesVacationsDetails"];
            var VacationsByRanksDetails = Vacations.Where(x => x.RankCategoryName == RankCategoryName).Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                VacationStartDate = y.VacationStartDate,
                VacationPeriod = y.VacationPeriod,
                VacationTypeName = y.VacationTypeName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = VacationsByRanksDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesVacationsAsDetails/{RankName}")]
        public JsonResult GetEmployeesVacationsAsDetails(string RankName)
        {
            IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> Vacation = (IQueryable<EmployeesVacationsBasedOnAssigngingsDTO>)Session["EmployeesVacationsDetails"];
            var VacationsAsDetails = Vacation
                     .Where(x => x.RankName == RankName)
                     .Select(y => new
                     {
                         EmployeeCodeNo = y.EmployeeCodeNo,
                         EmployeeNameAr = y.EmployeeNameAr,
                         OrganizationName = y.OrganizationName,
                         JobName = y.JobName,
                         RankCategoryName = y.RankCategoryName,
                         RankName = y.RankName,
                         VacationStartDate = y.VacationStartDate,
                         VacationPeriod = y.VacationPeriod,
                         VacationTypeName = y.VacationTypeName,
                         Sorting = y.Sorting
                     }).OrderByDescending(x => x.RankCategoryName)
                       .ThenByDescending(x => x.Sorting);

            return Json(new { data = VacationsAsDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeesDelegations
        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesDelegationsAsRanksCategories/{DelegationTypeID}/{FromDate}/{ToDate}/{OrganizationID}")]
        public JsonResult GetEmployeesDelegationsAsRanksCategories(int DelegationTypeID, DateTime FromDate, DateTime ToDate, int OrganizationID)
        {
            IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO> Delegations;
            Delegations = new BaseDelegationsBLL().GetEmployeesDelegationsBasedOnAssigningsAsRanksCategories(DelegationTypeID, FromDate, ToDate, OrganizationID);
            Session["EmployeesDelegationsDetails"] = Delegations;

            var DelegationsByRanksCategories = Delegations
                     .GroupBy(x => x.RankCategoryName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = DelegationsByRanksCategories }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeesDelegationsAsRanksCategoriesDetails()
        {
            IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO> DelegationsByRanksCategories = (IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO>)Session["EmployeesDelegationsDetails"];
            var DelegationsByRanksCategoriesDetails = DelegationsByRanksCategories.Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                DelegationStartDate = y.DelegationStartDate,
                DelegationPeriod = y.DelegationPeriod,
                DelegationTypeName = y.DelegationTypeName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = DelegationsByRanksCategoriesDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesDelegationsAsRanks/{RankCategoryName}")]
        public JsonResult GetEmployeesDelegationsAsRanks(string RankCategoryName)
        {
            IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO> Delegations = (IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO>)Session["EmployeesDelegationsDetails"];
            var DelegationsAsRanks = Delegations
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankCategoryName == RankCategoryName)
                     .GroupBy(x => x.RankName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = DelegationsAsRanks }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesDelegationsAsRanksDetails/{RankCategoryName}")]
        public JsonResult GetEmployeesDelegationsAsRanksDetails(string RankCategoryName)
        {
            IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO> Delegations = (IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO>)Session["EmployeesDelegationsDetails"];
            var DelegationsByRanksDetails = Delegations.Where(x => x.RankCategoryName == RankCategoryName).Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                DelegationStartDate = y.DelegationStartDate,
                DelegationPeriod = y.DelegationPeriod,
                DelegationTypeName = y.DelegationTypeName,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = DelegationsByRanksDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesDelegationsAsDetails/{RankName}")]
        public JsonResult GetEmployeesDelegationsAsDetails(string RankName)
        {
            IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO> Delegation = (IQueryable<EmployeesDelegationsBasedOnAssigngingsDTO>)Session["EmployeesDelegationsDetails"];
            var DelegationsAsDetails = Delegation
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankName == RankName)
                     .Select(y => new
                     {
                         EmployeeCodeNo = y.EmployeeCodeNo,
                         EmployeeNameAr = y.EmployeeNameAr,
                         OrganizationName = y.OrganizationName,
                         JobName = y.JobName,
                         RankCategoryName = y.RankCategoryName,
                         RankName = y.RankName,
                         DelegationStartDate = y.DelegationStartDate,
                         DelegationPeriod = y.DelegationPeriod,
                         DelegationTypeName = y.DelegationTypeName
                     });

            return Json(new { data = DelegationsAsDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeesOverTimes
        [HttpGet]
        [Route("{controller}/GetEmployeesOverTimesAsRanksCategories/{FromDate}/{ToDate}/{OrganizationID}")]
        public JsonResult GetEmployeesOverTimesAsRanksCategories(DateTime FromDate, DateTime ToDate, int OrganizationID)
        {
            IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> OverTimes;
            OverTimes = new OverTimesBLL().GetEmployeesOverTimesBasedOnAssigningsAsRanksCategories(FromDate, ToDate, OrganizationID);
            Session["EmployeesOverTimesDetails"] = OverTimes;

            var OverTimesByRanksCategories = OverTimes
                     .GroupBy(x => x.RankCategoryName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = OverTimesByRanksCategories }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeesOverTimesAsRanksCategoriesDetails()
        {
            IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> OverTimesByRanksCategories = (IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO>)Session["EmployeesOverTimesDetails"];
            var OverTimesByRanksCategoriesDetails = OverTimesByRanksCategories.Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                OverTimeStartDate = y.OverTimeStartDate,
                OverTimePeriod = y.OverTimePeriod,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.RankCategoryName)
              .ThenByDescending(x => x.Sorting);

            return Json(new { data = OverTimesByRanksCategoriesDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesOverTimesAsRanks/{RankCategoryName}")]
        public JsonResult GetEmployeesOverTimesAsRanks(string RankCategoryName)
        {
            IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> OverTimes = (IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO>)Session["EmployeesOverTimesDetails"];
            var OverTimesAsRanks = OverTimes
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankCategoryName == RankCategoryName)
                     .GroupBy(x => x.RankName)
                     .Select(y => new
                     {
                         KeyName = y.Key,
                         Value = y.Count()
                     });

            return Json(new { data = OverTimesAsRanks }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesOverTimesAsRanksDetails/{RankCategoryName}")]
        public JsonResult GetEmployeesOverTimesAsRanksDetails(string RankCategoryName)
        {
            IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> OverTimes = (IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO>)Session["EmployeesOverTimesDetails"];
            var OverTimesByRanksDetails = OverTimes.Where(x => x.RankCategoryName == RankCategoryName).Select(y => new
            {
                EmployeeCodeNo = y.EmployeeCodeNo,
                EmployeeNameAr = y.EmployeeNameAr,
                OrganizationName = y.OrganizationName,
                JobName = y.JobName,
                RankCategoryName = y.RankCategoryName,
                RankName = y.RankName,
                OverTimeStartDate = y.OverTimeStartDate,
                OverTimePeriod = y.OverTimePeriod,
                Sorting = y.Sorting
            }).OrderByDescending(x => x.Sorting);

            return Json(new { data = OverTimesByRanksDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("ManagerDashboard/GetEmployeesOverTimesAsDetails/{RankName}")]
        public JsonResult GetEmployeesOverTimesAsDetails(string RankName)
        {
            IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO> OverTime = (IQueryable<EmployeesOverTimesBasedOnAssigngingsDTO>)Session["EmployeesOverTimesDetails"];
            var OverTimesAsDetails = OverTime
                     .OrderByDescending(x => x.Sorting)
                     .Where(x => x.RankName == RankName)
                     .Select(y => new
                     {
                         EmployeeCodeNo = y.EmployeeCodeNo,
                         EmployeeNameAr = y.EmployeeNameAr,
                         OrganizationName = y.OrganizationName,
                         JobName = y.JobName,
                         RankCategoryName = y.RankCategoryName,
                         RankName = y.RankName,
                         OverTimeStartDate = y.OverTimeStartDate,
                         OverTimePeriod = y.OverTimePeriod
                     });

            return Json(new { data = OverTimesAsDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }


    [DataContract]
    public class DataPoint
    {
        public DataPoint(string Label, double Value)
        {
            this.Label = Label;
            this.Value = Value;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "Label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "Value")]
        public Nullable<double> Value = null;
    }
}