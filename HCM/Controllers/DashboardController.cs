using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HCM.Models;



namespace HCM.Controllers
{
    //[CustomAuthorize]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            //List<ProjectsBLL> ProjectsBLLList = new List<ProjectsBLL>();
            //if (Session["Projects"] != null)
            //    ProjectsBLLList = (List<ProjectsBLL>)Session["Projects"];

            //int GroupsCount = 0;
            //int UsersCount = 0;
            //foreach (var item in ProjectsBLLList)
            //{
            //    GroupsCount += item.GroupsCount;
            //    UsersCount += item.UsersCount;
            //}
            DashboardViewModel DashboardVM = new DashboardViewModel()
            {
                //ProjectsCount = ProjectsBLLList.Count,
                //GroupsCount = GroupsCount,
                //UsersCount = UsersCount
            };
            return PartialView("~/Views/Dashboard/_Index.cshtml", DashboardVM);
        }

        public ActionResult Test()
        {
            //List<ProjectsBLL> ProjectsBLLList = new List<ProjectsBLL>();
            //if (Session["Projects"] != null)
            //    ProjectsBLLList = (List<ProjectsBLL>)Session["Projects"];

            //int GroupsCount = 0;
            //int UsersCount = 0;
            //foreach (var item in ProjectsBLLList)
            //{
            //    GroupsCount += item.GroupsCount;
            //    UsersCount += item.UsersCount;
            //}
            DashboardViewModel DashboardVM = new DashboardViewModel()
            {
                //ProjectsCount = ProjectsBLLList.Count,
                //GroupsCount = GroupsCount,
                //UsersCount = UsersCount
            };
            Session["UserID"] = 0;
            //return PartialView("~/Views/Dashboard/_Index.cshtml", DashboardVM);
            return View();
        }

    }
}
