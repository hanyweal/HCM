using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class StopWorksByEndedStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter StopWorkEnded = new ReportParameter("StopWorkEnded", Request.QueryString["StopWorkEnded"] != "" ? Request.QueryString["StopWorkEnded"].ToString() : null);
            ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", Request.QueryString["EmployeeCodeID"] != "" ? Request.QueryString["EmployeeCodeID"].ToString() : null);
            ReportParameter ReportTitle = new ReportParameter("ReportTitle", Request.QueryString["ReportTitle"] != "" ? Request.QueryString["ReportTitle"].ToString() : null);

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "StopWorksByEndedStatusAndEmployeeCode";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, StopWorkEnded, EmployeeCodeID, ReportTitle);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
