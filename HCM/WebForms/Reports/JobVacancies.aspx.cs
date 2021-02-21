using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class JobVacancies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter RankID = new ReportParameter("RankID", Request.QueryString["ID"].ToString());

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "JobVacanciesByRank";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, RankID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
