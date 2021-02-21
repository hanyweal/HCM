using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class JobVacanciesByRankAndCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter RankID = new ReportParameter("RankID", Request.QueryString["RankID"] != "" ? Request.QueryString["RankID"].ToString() : null);
            ReportParameter JobCategoryID = new ReportParameter("JobCategoryID", Request.QueryString["JobCategoryID"] != "" ? Request.QueryString["JobCategoryID"].ToString() : null);

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "JobVacanciesByRankAndCategory";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, RankID, JobCategoryID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
