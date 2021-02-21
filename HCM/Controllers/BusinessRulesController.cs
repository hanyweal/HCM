using HCM.Models;
using HCMBLL;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class BusinessRulesController : Controller
    {
        //
        // GET: /BusinessRules/
        [ChildActionOnly]
        public ActionResult Index(int id)
        {
            List<BusinessRulesBLL> BusinessRulesBLLList = new BusinessRulesBLL().GetByBusinessSubCategory(id);
            List<BusinessRulesViewModel> BusinessRulesVMList = new List<BusinessRulesViewModel>();
            foreach (var item in BusinessRulesBLLList)
                BusinessRulesVMList.Add(new BusinessRulesViewModel() { BusinessRuleAR = item.BusinessRuleAr });

            return PartialView("~/Views/Shared/_BusinessRulesList.cshtml", BusinessRulesVMList);
        }
    }
}