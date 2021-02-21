using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class TeachersController : BaseController
    {
        // GET: Teachers
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTeachers()
        {
            var data = new TeachersBLL().GetTeachers().Select(x => new
            {
                TeacherID = x.TeacherID,
                EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy.Employee.EmployeeNameAr
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["TeacherID"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Create(TeachersViewModel TeacherVM)
        {
            TeachersBLL TeacherBLL = new TeachersBLL()
            {
                StartDate = TeacherVM.StartDate,
                EndDate = TeacherVM.EndDate,
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(TeacherVM.EmployeeCodeID.Value),
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = UserIdentity.EmployeeCodeID },
            };
            Result result = TeacherBLL.Add();
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            if ((System.Type)result.EnumType == typeof(TeachersValidationEnum))
            {
                if (result.EnumMember == TeachersValidationEnum.Done.ToString())
                {
                    TeacherVM.TeacherID = ((TeachersBLL)result.Entity).TeacherID;
                }
                else if (result.EnumMember == TeachersValidationEnum.RejectedBecauseOfEndDateShouldNotBeLessThanStartDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEndDateShouldBeMoreThanStartDateText);
                }
                else if (result.EnumMember == TeachersValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationBeforeHiringText);
                }
            }

            return Json(new { data = TeacherVM.TeacherID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            TeachersViewModel TeacherVM = GetTeacherByTeacherID(id);
            return View(TeacherVM);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditTeachers(TeachersViewModel TeacherVM)
        {
            TeachersBLL TeacherBLL = new TeachersBLL()
            {
                TeacherID = TeacherVM.TeacherID,
                StartDate = TeacherVM.StartDate,
                EndDate = TeacherVM.EndDate,
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(TeacherVM.EmployeeCodeID.Value),
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = UserIdentity.EmployeeCodeID },
            };
            Result result = TeacherBLL.Update();
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            if ((System.Type)result.EnumType == typeof(TeachersValidationEnum))
            {
                if (result.EnumMember == TeachersValidationEnum.Done.ToString())
                {
                    TeacherVM.TeacherID = ((TeachersBLL)result.Entity).TeacherID;
                }
                else if (result.EnumMember == TeachersValidationEnum.RejectedBecauseOfEndDateShouldNotBeLessThanStartDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEndDateShouldBeMoreThanStartDateText);
                }
                else if (result.EnumMember == TeachersValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationBeforeHiringText);
                }
            }
            return View(this.GetTeacherByTeacherID(TeacherVM.TeacherID));
            //return View(LenderVM);
        }

        private TeachersViewModel GetTeacherByTeacherID(int id)
        {
            TeachersBLL TeacherBLL = new TeachersBLL().GetByTeacherID(id);
            TeachersViewModel TeacherVM = new TeachersViewModel();
            if (TeacherBLL != null)
            {
                TeacherVM.TeacherID = TeacherBLL.TeacherID;
                TeacherVM.StartDate = TeacherBLL.StartDate;
                TeacherVM.EndDate = TeacherBLL.EndDate;

                TeacherVM.Employee = new EmployeesViewModel()
                {
                    EmployeeCodeID = TeacherBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = TeacherBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = TeacherBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr
                };
            }
            return TeacherVM;
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            TeachersViewModel TeacherVM = GetTeacherByTeacherID(id);
            return View(TeacherVM);
        }

        [HttpDelete]
        [IgnoreModelProperties("StartDate,EndDate,EmployeeCareerHistoryID")]
        public ActionResult Delete(TeachersViewModel TeacherVM)
        {
            TeachersBLL TeacherBLL = new TeachersBLL()
            {
                TeacherID = TeacherVM.TeacherID,
                LoginIdentity = UserIdentity
            };
            Result result = TeacherBLL.Remove();
            if ((System.Type)result.EnumType == typeof(TeachersValidationEnum))
            {
                // if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfActivePreviousLender.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.MustEndPreviousLenderText + "NewLine" + "تاريخ اخر تكليف : " + assgining.LenderStartDate.ToShortDateString());
                //}
            }
            return View();
        }
    }
}