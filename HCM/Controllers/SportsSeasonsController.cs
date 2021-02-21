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
    public class SportsSeasonsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [Route("SportsSeasons/GetByMaturityYearID/{MaturityYearID}")]
        public JsonResult GetByMaturityYearID(int MaturityYearID)
        {
            return Json(new
            {
                data = ((new SportsSeasonsBLL().GetByMaturityYearID(MaturityYearID))).Select(x => new
                {
                    SportsSeasonID = x.SportsSeasonID,
                    SportsSeasonDescription = x.SportsSeasonDescription,
                    SportsSeasonStartDate = x.SportsSeasonStartDate,
                    SportsSeasonPeriod = x.SportsSeasonPeriod,
                    SportsSeasonEndDate = x.SportsSeasonEndDate,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create(int id)
        {
            return View(new SportsSeasonsViewModel() { MaturityYearBalance = new MaturityYearsBalancesBLL().GetByMaturityYearID(id) });
        }

        [HttpPost]
        [IgnoreModelProperties("SportsSeasonPeriod")]
        public ActionResult Create(SportsSeasonsViewModel SportsSeasonVM)
        {
            //--== HolidaySetting Master DataBind ===
            SportsSeasonsBLL SportsSeason = new SportsSeasonsBLL();
            SportsSeason.SportsSeasonStartDate = SportsSeasonVM.SportsSeasonStartDate.Value;
            SportsSeason.SportsSeasonEndDate = SportsSeasonVM.SportsSeasonEndDate.Value;
            SportsSeason.SportsSeasonDescription = SportsSeasonVM.SportsSeasonDescription;
            SportsSeason.MaturityYear = SportsSeasonVM.MaturityYearBalance;
            SportsSeason.LoginIdentity = UserIdentity;
            Result result = SportsSeason.Add();
            if (result.EnumMember == SportsSeasonsValidationEnum.Done.ToString())
            {

            }
            return Json(new { SportsSeasonID = SportsSeason.SportsSeasonID, MaturityYearID = SportsSeason.MaturityYear.MaturityYearID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetBySportsSeasonID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("MaturityYearBalance")]
        public ActionResult Edit(SportsSeasonsViewModel SportsSeasonVM)
        {
            SportsSeasonsBLL SportsSeason = new SportsSeasonsBLL();
            SportsSeason.SportsSeasonID = SportsSeasonVM.SportsSeasonID;
            SportsSeason.SportsSeasonStartDate = SportsSeasonVM.SportsSeasonStartDate.Value;
            SportsSeason.SportsSeasonEndDate = SportsSeasonVM.SportsSeasonEndDate.Value;
            SportsSeason.SportsSeasonDescription = SportsSeasonVM.SportsSeasonDescription;
            SportsSeason.MaturityYear = SportsSeasonVM.MaturityYearBalance;
            SportsSeason.LoginIdentity = UserIdentity;
            Result result = SportsSeason.Update();

            if (result.EnumMember == SportsSeasonsValidationEnum.RejectedBecauseOfAlreadyCreatedVacationOnTheSeason.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationNoChanceToEditAlreadyCreatedVacationOnTheSeasonText);
            }
            return Json(new { SportsSeasonID = SportsSeason.SportsSeasonID, MaturityYearID = SportsSeason.MaturityYear.MaturityYearID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetBySportsSeasonID(id));
        }

        [HttpDelete]
        [ActionName("Delete")]
        [IgnoreModelProperties("MaturityYearBalance,SportsSeasonDescription,SportsSeasonStartDate,SportsSeasonEndDate,SportsSeasonPeriod")]
        public ActionResult Delete(SportsSeasonsViewModel SportsSeasonVM)
        {
            SportsSeasonsBLL SportsSeason = new SportsSeasonsBLL();
            SportsSeason.LoginIdentity = UserIdentity;
            SportsSeason.MaturityYear = SportsSeasonVM.MaturityYearBalance;
            SportsSeason.Remove((int)SportsSeasonVM.SportsSeasonID);
            return Json(new { SportsSeasonID = SportsSeason.SportsSeasonID, MaturityYearID = SportsSeason.MaturityYear.MaturityYearID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private SportsSeasonsViewModel GetBySportsSeasonID(int id)
        {
            SportsSeasonsBLL SportsSeasonBLL = (new SportsSeasonsBLL()).GetBySportsSeasonID(id);
            SportsSeasonsViewModel SportsSeasonVM = new SportsSeasonsViewModel();
            if (SportsSeasonBLL != null)
            {
                SportsSeasonVM.SportsSeasonDescription = SportsSeasonBLL.SportsSeasonDescription;
                SportsSeasonVM.SportsSeasonStartDate = SportsSeasonBLL.SportsSeasonStartDate;
                SportsSeasonVM.SportsSeasonEndDate = SportsSeasonBLL.SportsSeasonEndDate;
                SportsSeasonVM.SportsSeasonPeriod = SportsSeasonBLL.SportsSeasonPeriod;
                SportsSeasonVM.SportsSeasonID = SportsSeasonBLL.SportsSeasonID;
                SportsSeasonVM.MaturityYearBalance = SportsSeasonBLL.MaturityYear;

            }
            return SportsSeasonVM;
        }
    }
}