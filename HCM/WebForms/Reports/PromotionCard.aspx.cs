using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class PromotionCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", Request.QueryString["ID"].ToString());

            //string ReportName = string.Empty;
            //string strFileName = string.Empty;
            //ReportServicesMethod RSM = new ReportServicesMethod();
            //if (!Page.IsPostBack)
            //{
            //    ReportName = "PromotionCard";
            //    strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, EmployeeCodeID);
            //    if (!string.IsNullOrEmpty(strFileName))
            //        Response.Redirect(strFileName);
            //}


            ReportParameter PromotionPeriodID = new ReportParameter("PromotionPeriodID", Request.QueryString["PromotionPeriodID"].ToString());
            ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", Request.QueryString["EmployeeCodeID"].ToString());

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "PromotionCard";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, EmployeeCodeID, PromotionPeriodID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}